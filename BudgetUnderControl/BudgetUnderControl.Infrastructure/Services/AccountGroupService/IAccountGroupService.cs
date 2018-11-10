using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetUnderControl.Model.Services
{
    public interface IAccountGroupService
    {
        Task<ICollection<AccountGroupItemDTO>> GetAccountGroupsAsync();
    }
}
