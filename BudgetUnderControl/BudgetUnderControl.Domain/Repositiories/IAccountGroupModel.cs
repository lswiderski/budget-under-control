using BudgetUnderControl.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetUnderControl.Domain.Repositiories
{
    public interface IAccountGroupRepository
    {
        Task<ICollection<AccountGroupItemDTO>> GetAccountGroups();
    }
}
