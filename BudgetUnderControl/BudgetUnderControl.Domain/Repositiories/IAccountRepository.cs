using BudgetUnderControl.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetUnderControl.Domain.Repositiories
{
    public interface IAccountRepository
    {
        ICollection<AccountListItemDTO> GetAccounts();
        void AddAccount(AddAccountDTO account);
        Task<Account> GetAccountAsync(int id);
        Task<AccountDetailsDTO> GetAccountDetails(int id, DateTime fromDate, DateTime toDate);
        void EditAccount(EditAccountDTO vm);
        void RemoveAccount(int id);
        Task ActivateAccountAsync(int id);
        void DeactivateAccount(int id);
        Task UpdateAsync(Account account);
        Task<decimal> GetActualBalanceAsync(int accountId);
    }
}
