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

        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string ActualRange
        {
            get
            {
                return string.Format("{0}, {1}", FromDate.ToString("MMMM"), FromDate.Year);
            }
        }

        public TransactionsViewModel(ITransactionModel transactionModel)
        {
            this.transactionModel = transactionModel;

            var now = DateTime.UtcNow;
            FromDate = new DateTime(now.Year, now.Month, 1);
            ToDate = new DateTime(now.Year, now.Month, DateTime.DaysInMonth(now.Year, now.Month));
        }

        public async void LoadTransactions()
        {
            Transactions = await transactionModel.GetTransactions(FromDate, ToDate);
        }

        public void SetNextMonth()
        {
            FromDate = new DateTime(FromDate.Year, FromDate.Month, 1).AddMonths(1);
            ToDate = new DateTime(FromDate.Year, FromDate.Month, DateTime.DaysInMonth(FromDate.Year, FromDate.Month));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ActualRange)));
            LoadTransactions();
        }

        public void SetPreviousMonth()
        {
            FromDate = new DateTime(FromDate.Year, FromDate.Month, 1).AddMonths(-1);
            ToDate = new DateTime(FromDate.Year, FromDate.Month, DateTime.DaysInMonth(FromDate.Year, FromDate.Month));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ActualRange)));
            LoadTransactions();
        }

    }
}
