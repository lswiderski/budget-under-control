using BudgetUnderControl.Common.Enums;
using BudgetUnderControl.Common.Contracts;
using BudgetUnderControl.Domain;
using BudgetUnderControl.Domain.Repositiories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BudgetUnderControl.Infrastructure.Services;

namespace BudgetUnderControl.Model
{
    public class AccountRepository : BaseModel, IAccountRepository
    {
        private readonly IUserIdentityContext userIdentityContext;

        public AccountRepository(IContextFacade context, IUserIdentityContext userIdentityContext) : base(context)
        {
            this.userIdentityContext = userIdentityContext;
        }

        public async Task<IEnumerable<Account>> GetAccountsAsync(bool? active = null)
        {
            var query = this.Context.Accounts.AsQueryable();

            if(active.HasValue)
            {
                query = query.Where(a => a.IsActive == active).AsQueryable();
            }

            var accounts = await (from account in query
                                  join currency in this.Context.Currencies on account.CurrencyId equals currency.Id
                                  where account.IsActive == true
                                  && account.OwnerId == userIdentityContext.UserId
                                  select account)
                                 .Include(p => p.Currency)
                                 .ToListAsync();
            return accounts;
        }

        public async Task<IEnumerable<Account>> GetAllAccountsAsync(bool? active = null)
        {
            var query = this.Context.Accounts.AsQueryable();

            if (active.HasValue)
            {
                query = query.Where(a => a.IsActive == active).AsQueryable();
            }
            var accounts = await (from account in query
                                  join currency in this.Context.Currencies on account.CurrencyId equals currency.Id
                                  where account.IsActive == true
                                  && account.OwnerId == userIdentityContext.UserId
                                  select account)
                                 .Include(p => p.Currency)
                                 .ToListAsync();
            return accounts;
        }

        public async Task AddAccountAsync(Account account)
        {
            this.Context.Accounts.Add(account);
            await this.Context.SaveChangesAsync();
        }

        public async Task RemoveAccountsAsync(IEnumerable<Account> accounts)
        {
            this.Context.Accounts.RemoveRange(accounts);
            await this.Context.SaveChangesAsync();
        }

        public async Task<Account> GetAccountAsync(int id)
        {
            var acc = await (from account in this.Context.Accounts
                             join currency in this.Context.Currencies on account.CurrencyId equals currency.Id
                             where account.Id == id
                             select account
                       ).Include(p => p.Currency)
                       .FirstOrDefaultAsync();

            return acc;
        }

        public async Task UpdateAsync(Account account)
        {
            this.Context.Accounts.Update(account);
            await this.Context.SaveChangesAsync();
        }

        public async Task<decimal> GetActualBalanceAsync(int accountId)
        {
            var isCard = IsSubCardAccount(accountId);

            if (isCard)
            {
                accountId = this.GetParentAccountId(accountId).Value;
            }

            var accounts = await this.GetSubAccountsAsync(new List<int> { accountId }, true);
            accounts.Add(accountId);
            accounts = accounts.Distinct().ToList();
            var transactions = await this.Context.Transactions.Where(x => accounts.Contains(x.AccountId)).Select(x => (decimal)x.Amount).ToListAsync();
            var balance = transactions.Sum(x => (decimal)x);

            return balance;
        }

        public async Task<decimal> GetIncomeAsync(int accountId, DateTime fromDate, DateTime toDate)
        {
            var isCard = IsSubCardAccount(accountId);

            if (isCard)
            {
                accountId = this.GetParentAccountId(accountId).Value;
            }

            var accounts = await this.GetSubAccountsAsync(new List<int> { accountId }, true);
            accounts.Add(accountId);
            accounts = accounts.Distinct().ToList();

            var balance = this.Context.Transactions.Where(x => accounts.Contains(x.AccountId) && x.Amount > 0 && x.Date >= fromDate && x.Date <= toDate).Select(x => (decimal)x.Amount).ToList().Sum(x => (decimal)x);
            return balance;
        }

        public async Task<decimal> GetExpenseAsync(int accountId, DateTime fromDate, DateTime toDate)
        {
            var isCard = IsSubCardAccount(accountId);

            if (isCard)
            {
                accountId = this.GetParentAccountId(accountId).Value;
            }

            var accounts = await this.GetSubAccountsAsync(new List<int> { accountId }, true);
            accounts.Add(accountId);
            accounts = accounts.Distinct().ToList();

            var balance = this.Context.Transactions.Where(x => accounts.Contains(x.AccountId) && x.Amount < 0 && x.Date >= fromDate && x.Date <= toDate).Select(x => (decimal)x.Amount).ToList().Sum(x => (decimal)x);
            return balance;
        }

        public async Task BalanceAdjustmentAsync(int accountId, decimal targetBalance)
        {
            decimal actualBalance = await GetActualBalanceAsync(accountId);

            if (!decimal.Equals(actualBalance, targetBalance))
            {
                decimal amount = (decimal.Subtract(targetBalance, actualBalance));
                var user = await this.Context.Users.FirstOrDefaultAsync();
                var type = Math.Sign(amount) < 0 ? TransactionType.Expense : TransactionType.Income;
                var transaction = Transaction.Create(accountId, type, amount, DateTime.UtcNow, "BalanceAdjustment", string.Empty, user.Id);

                this.Context.Transactions.Add(transaction);
                await this.Context.SaveChangesAsync();
            }
        }

        public async Task<List<int>> GetSubAccountsAsync(IEnumerable<int> accountsIds, bool? active = null)
        {
            var query = this.Context.Accounts.AsQueryable();

            if (active.HasValue)
            {
                query = query.Where(a => a.IsActive == active).AsQueryable();
            }

            var subAccounts = await query
                .Where(x => x.ParentAccountId.HasValue 
                && accountsIds.Contains(x.ParentAccountId.Value)
                && x.IsActive == true)
                .Select(x => x.Id)
                .ToListAsync();
            return subAccounts;
        }

        private bool IsSubCardAccount(int accountId)
        {
            var result = this.Context.Accounts.Any(x => x.Id == accountId && x.ParentAccountId.HasValue && x.Type == AccountType.Card);
            return result;
        }

        private int? GetParentAccountId(int accountId)
        {
            var result = this.Context.Accounts.Where(x => x.Id == accountId).Select(x => x.ParentAccountId).FirstOrDefault();
            return result;
        }
    }
}
