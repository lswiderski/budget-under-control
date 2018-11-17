using BudgetUnderControl.Common.Contracts;
using BudgetUnderControl.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetUnderControl.ViewModel
{
    public interface IAccountsViewModel
    {
        ICollection<AccountListItemDTO> Accounts { get; }
        AccountListItemDTO SelectedAccount { get; set; }

        Task LoadAccounts();
    }
}
