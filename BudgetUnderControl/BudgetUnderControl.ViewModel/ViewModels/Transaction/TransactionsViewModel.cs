using BudgetUnderControl.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetUnderControl.ViewModel
{
    public class TransactionsViewModel : ITransactionsViewModel, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        ITransactionModel transactionModel;
        public TransactionListItemDTO SelectedTransaction { get; set; }

        ICollection<TransactionListItemDTO> transactions;
        public ICollection<TransactionListItemDTO> Transactions
        {
            get
            {
                return transactions;
            }
            set
            {
                if (transactions != value)
                {
                    transactions = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Transactions)));
                }

            }
        }

        public TransactionsViewModel(ITransactionModel transactionModel)
        {
            this.transactionModel = transactionModel;
        }

        public async void LoadTransactions()
        {
            Transactions = await transactionModel.GetTransactions();
        }
    }
}
