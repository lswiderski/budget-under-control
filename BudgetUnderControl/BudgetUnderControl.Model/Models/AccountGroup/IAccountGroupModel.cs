using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetUnderControl.Model
{
    public interface IAccountGroupModel
    {
        Task<ICollection<AccountGroupItemDTO>> GetAccountGroups();
    }
}
