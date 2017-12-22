using BudgetUnderControl.Common;
using BudgetUnderControl.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetUnderControl.ViewModel
{
    public interface ITransactionsViewModel
    {
        ObservableCollection<ObservableGroupCollection<string, TransactionListItemDTO>> Transactions { get; set; }
        TransactionListItemDTO SelectedTransaction { get; set; }
        string ActualRange { get; }

        void LoadTransactions();
        void SetNextMonth();
        void SetPreviousMonth();
    }
}
