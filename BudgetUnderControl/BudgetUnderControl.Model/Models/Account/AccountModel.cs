using BudgetUnderControl.Common.Enums;
using BudgetUnderControl.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetUnderControl.Model
{
    public class AccountModel : BaseModel, IAccountModel
    {

        IContextConfig contextConfig;
        ITransactionModel transactionModel;
        public AccountModel(IContextConfig contextConfig, ITransactionModel transactionModel) : base(contextConfig)
        {
            this.contextConfig = contextConfig;

            this.transactionModel = transactionModel;
        }

        
        public async Task<ICollection<AccountListItemDTO>> GetAccounts()
        {
            var list = (from account in this.Context.Accounts
                        join currency in this.Context.Currencies on account.CurrencyId equals currency.Id
                        select new AccountListItemDTO
                        {
                             Currency = currency.Code,
                             CurrencyId = currency.Id,
                             CurrencySymbol = currency.Symbol,
                             Id = account.Id,
                             IsIncludedInTotal = account.IsIncludedToTotal,
                             Name = account.Name,
                             Balance = this.Context.Transactions.Where(x => x.AccountId == account.Id).Sum(x => x.Amount),
                        }
                        ).ToListAsync();

            return await list;
        }

        public async void AddAccount(AddAccountDTO vm)
        {
            var account = new Account
            {
                AccountGroupId = vm.AccountGroupId,
                Comment = vm.Comment,
                CurrencyId = vm.CurrencyId,
                IsIncludedToTotal = vm.IsIncludedInTotal,
                Name = vm.Name

            };
            this.Context.Accounts.Add(account);
            await this.Context.SaveChangesAsync();
            this.BalanceAdjustment(account.Id, vm.Amount);
        }

        public async Task<EditAccountDTO> GetAccount(int id)
        {
            var acc = (from account in this.Context.Accounts
                       join currency in this.Context.Currencies on account.CurrencyId equals currency.Id
                       where account.Id == id
                       select new EditAccountDTO
                       {
                           Currency = currency.Code,
                           CurrencyId = currency.Id,
                           CurrencySymbol = currency.Symbol,
                           Id = account.Id,
                           IsIncludedInTotal = account.IsIncludedToTotal,
                           Name = account.Name,
                           Comment = account.Comment,
                            AccountGroupId = account.AccountGroupId,
                           Amount = this.Context.Transactions.Where(x => x.AccountId == account.Id).Sum(x => x.Amount),
                       }
                        ).FirstOrDefaultAsync();

            return await acc;
        }

        public async void EditAccount(EditAccountDTO vm)
        {
            var account = await this.Context.Accounts.Where(x => x.Id == vm.Id).FirstOrDefaultAsync();
            account.AccountGroupId = vm.AccountGroupId;
            account.Comment = vm.Comment;
            account.CurrencyId = vm.CurrencyId;
            account.IsIncludedToTotal = vm.IsIncludedInTotal;
            account.Name = vm.Name;

            this.Context.SaveChanges();
            this.BalanceAdjustment(account.Id, vm.Amount);
        }

        public async void RemoveAccount(int id)
        {
            var transactions = this.Context.Transactions.Where(x => x.AccountId == id).ToList();
            this.Context.RemoveRange(transactions);
            this.Context.SaveChanges();
            var account = await this.Context.Accounts.Where(x => x.Id == id).FirstOrDefaultAsync();
            this.Context.Remove<Account>(account);
            this.Context.SaveChanges();
        }

        private decimal GetActualBalance(int accountId)
        {
            var balance = this.Context.Transactions.Where(x => x.AccountId == accountId).Sum(x => x.Amount);
            return balance;
        }

        private void BalanceAdjustment(int accountId, decimal targetBalance)
        {
            var actualBalance = GetActualBalance(accountId);

            var amount = targetBalance - actualBalance;
            Math.Sign(amount);
            var transactionDTO = new AddTransactionDTO
            {
                 AccountId = accountId,
                 CreatedOn = DateTime.UtcNow,
                 Comment = string.Empty,
                 Name = "BalanceAdjustment",
                 Amount = amount,
                 Type = Math.Sign(amount)< 0 ? TransactionType.Expense : TransactionType.Income,
            };

            transactionModel.AddTransaction(transactionDTO);
        }

    }
}
