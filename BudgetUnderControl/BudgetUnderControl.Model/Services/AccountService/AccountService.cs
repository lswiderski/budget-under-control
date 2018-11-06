using BudgetUnderControl.Contracts.Models;
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

        public async Task ActivateAccountAsync(int id)
        {
            var account = await accountRepository.GetAccountAsync(id);
            account.IsActive = true;

            await accountRepository.UpdateAsync(account);
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
    }
}
