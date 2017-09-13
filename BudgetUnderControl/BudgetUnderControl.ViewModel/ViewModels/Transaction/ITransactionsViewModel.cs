using BudgetUnderControl.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetUnderControl.ViewModel
{
    public interface ITransactionsViewModel
    {
        ICollection<TransactionListItemDTO> Transactions { get; set; }
        TransactionListItemDTO SelectedTransaction { get; set; }
        string ActualRange { get; }

        void LoadTransactions();
        void SetNextMonth();
        void SetPreviousMonth();
    }
}
