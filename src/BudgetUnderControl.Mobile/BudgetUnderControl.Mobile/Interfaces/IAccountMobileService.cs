using BudgetUnderControl.Common.Contracts;
using BudgetUnderControl.CommonInfrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BudgetUnderControl.Mobile
{
    public interface IAccountMobileService : IAccountService
    {
        Task<ICollection<AccountListItemDTO>> GetAccountsForSelect(int? includeId = null);
    }
}
