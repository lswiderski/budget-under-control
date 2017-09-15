using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetUnderControl.Model
{
   public interface IAccountModel
    {
        ICollection<AccountListItemDTO> GetAccounts();
        void AddAccount(AddAccountDTO vm);
        Task<EditAccountDTO> GetAccount(int id);
        Task<AccountDetailsDTO> GetAccountDetails(int id, DateTime fromDate, DateTime toDate);
        void EditAccount(EditAccountDTO vm);
        void RemoveAccount(int id);
    }
}
