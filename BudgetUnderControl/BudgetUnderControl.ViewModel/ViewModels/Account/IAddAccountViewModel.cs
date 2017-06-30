using BudgetUnderControl.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetUnderControl.ViewModel
{
    public interface IAddAccountViewModel
    {
        string Name { get; set; }
        string Comment { get; set; }
        bool IsInTotal { get; set; }
        string Amount { get; set; }
        int SelectedCurrencyIndex { get; set; }
        int SelectedAccountGroupIndex { get; set; }
        List<AccountGroupItemDTO> AccountGroups { get; }
        List<CurrencyDTO> Currencies { get; }

        void AddAccount();
    }
}
