using BudgetUnderControl.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetUnderControl.Model.Services
{
    public interface IAccountService
    {
        Task ActivateAccountAsync(int id);
        Task DeactivateAccountAsync(int id);
        Task RemoveAccountAsync(int id);
        Task<EditAccountDTO> GetEditAccountDTOAsync(int id);
        Task<ICollection<AccountListItemDTO>> GetAccountsWithBalanceAsync();
        Task<AccountDetailsDTO> GetAccountDetailsAsync(int id, DateTime fromDate, DateTime toDate);

        Task AddAccountAsync(AddAccountDTO account);
        Task EditAccountAsync(EditAccountDTO vm);
    }
}
