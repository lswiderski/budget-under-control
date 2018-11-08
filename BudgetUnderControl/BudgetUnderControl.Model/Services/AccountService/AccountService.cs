using BudgetUnderControl.Common.Enums;
using BudgetUnderControl.Contracts.Models;
using BudgetUnderControl.Domain;
using BudgetUnderControl.Domain.Repositiories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetUnderControl.Model.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository accountRepository;

        public AccountService(IAccountRepository accountRepository)
        {
            this.accountRepository = accountRepository;
        }

        public async Task<EditAccountDTO> GetEditAccountDTOAsync(int id)
        {
            var account = await accountRepository.GetAccountAsync(id);
            var balance = await accountRepository.GetActualBalanceAsync(id);
            var dto = new EditAccountDTO
            {
                Id = account.Id,
                Name = account.Name,
                Comment = account.Comment,
                IsActive = account.IsActive,
                Type = account.Type,
                Order = account.Order,
                Amount = balance,
                Currency = account.Currency.Code,
                CurrencySymbol = account.Currency.Symbol,
                CurrencyId = account.CurrencyId,
                IsIncludedInTotal = account.IsIncludedToTotal,
                AccountGroupId = account.AccountGroupId,
                ParentAccountId = account.ParentAccountId
            };
            return dto;
        }

        public async Task<ICollection<AccountListItemDTO>> GetAccountsWithBalanceAsync()
        {
            var accounts = await accountRepository.GetAccountsAsync();

            var accountsWithBalance = accounts.Select(y => new AccountListItemDTO
            {
                Id = y.Id,
                Currency = y.Currency.Code,
                CurrencyId = y.CurrencyId,
                CurrencySymbol = y.Currency.Symbol,
                IsIncludedInTotal = y.IsIncludedToTotal,
                Name = y.Name,
                ParentAccountId = y.ParentAccountId,
                
            }).ToList();
            accountsWithBalance.ForEach(async x => { x.Balance = await accountRepository.GetActualBalanceAsync(x.Id); });

            return accountsWithBalance;
        }

        public async Task<AccountDetailsDTO> GetAccountDetailsAsync(TransactionsFilter filter)
        {
            if(filter == null || !filter.AccountId.HasValue
                || !filter.FromDate.HasValue || !filter.ToDate.HasValue)
            {
                throw new ArgumentNullException();
            }
            var account = await accountRepository.GetAccountAsync(filter.AccountId.Value);

            var dto = new AccountDetailsDTO
            {
                Currency = account.Currency.Code,
                CurrencyId = account.CurrencyId,
                CurrencySymbol = account.Currency.Symbol,
                Id = account.Id,
                IsIncludedInTotal = account.IsIncludedToTotal,
                Name = account.Name,
                Comment = account.Comment,
                AccountGroupId = account.AccountGroupId,
                Type = account.Type,
                ParentAccountId = account.ParentAccountId,
                Order = account.Order,
                Income = await accountRepository.GetIncomeAsync(account.Id, filter.FromDate.Value, filter.ToDate.Value),
                Expense = await accountRepository.GetExpenseAsync(account.Id, filter.FromDate.Value, filter.ToDate.Value)
            };

            return dto;
        }

        public async Task AddAccountAsync(AddAccountDTO dto)
        {
            var account = Account.Create(dto.Name, dto.CurrencyId, dto.AccountGroupId, dto.IsIncludedInTotal, dto.Comment, dto.Order, dto.Type, dto.ParentAccountId, true);
            await accountRepository.AddAccountAsync(account);
            
            if(account.Id <= 0)
            {
                throw new Exception();
            }

            if (account.Type != AccountType.Card)
            {
                await this.accountRepository.BalanceAdjustmentAsync(account.Id, dto.Amount);
            }
        }

        public async Task EditAccountAsync(EditAccountDTO dto)
        {
            var account = await accountRepository.GetAccountAsync(dto.Id);
            account.Edit(dto.Name, dto.CurrencyId, dto.AccountGroupId, dto.IsIncludedInTotal, dto.Comment, dto.Order, dto.Type, dto.ParentAccountId, dto.IsActive);
            await accountRepository.UpdateAsync(account);

            if (account.Type != AccountType.Card)
            {
                await this.accountRepository.BalanceAdjustmentAsync(account.Id, dto.Amount);
            }  
        }

        public async Task ActivateAccountAsync(int id)
        {
            var account = await accountRepository.GetAccountAsync(id);
            account.SetActive(true);

            await accountRepository.UpdateAsync(account);
        }

        public async Task DeactivateAccountAsync(int id)
        {
            var account = await accountRepository.GetAccountAsync(id);
            account.SetActive(false);

            await accountRepository.UpdateAsync(account);
        }

        public async Task RemoveAccountAsync(int id)
        {
            //temporary no removing AccountAvailable
            await this.DeactivateAccountAsync(id);

            /*
              var transactions = this.Context.Transactions.Where(x => x.AccountId == id).ToList();

            if(IsSubCardAccount(id))
            {
                var parentAccountId = GetParentAccountId(id).Value;
                foreach (var transaction in transactions)
                {
                    transaction.AccountId = parentAccountId;
                }
            }
            else
            {
                this.Context.RemoveRange(transactions);
            }

            
            this.Context.SaveChanges();
            var account = await this.Context.Accounts.Where(x => x.Id == id).FirstOrDefaultAsync();
            this.Context.Remove<Account>(account);
            this.Context.SaveChanges();
             */
        }
    }
}
