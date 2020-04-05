using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetUnderControl.MobileDomain.Repositiories
{
    public interface IAccountGroupRepository
    {
        Task<ICollection<AccountGroup>> GetAccountGroupsAsync();
        Task<AccountGroup> GetAccountGroupAsync(string externalId);
        Task UpdateAsync(AccountGroup accountGroup);
        Task AddAccountGroupAsync(AccountGroup accountGroup);
    }
}
