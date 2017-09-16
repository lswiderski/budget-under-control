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

        private string income;
        public string Income
        {
            get => income;
            set
            {
                if (income != value)
                {
                    income = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Income)));
                }
            }
        }

        private string expense;
        public string Expense
        {
            get => expense;
            set
            {
                if (expense != value)
                {
                    expense = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Expense)));
                }
            }
        }

        private string numberOfTransactions;
        public string NumberOfTransactions
        {
            get => numberOfTransactions;
            set
            {
                if (numberOfTransactions != value)
                {
                    numberOfTransactions = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NumberOfTransactions)));
                }
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
            SetIncomeExpense();
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

        private void SetIncomeExpense()
        {
            decimal _income = 0m;
            decimal _expense = 0m;
            int noTransactions = 0;
            foreach (var item in Transactions)
            {
                decimal _value = 0;

                switch (item.CurrencyCode)
                { 
                    case "PLN": _value = item.Value * 1m;
                break;
                    case "USD": _value = item.Value * 3.66m;
                break;
                    case "EUR": _value = item.Value * 4.26m;
                break;
                    case "THB": _value = item.Value * 0.11m;
                break;
                default:
                        break;
                }

                if(item.Value > 0)
                {
                    _income += _value;
                }
                else
                {
                    _expense += _value;
                }
                noTransactions++;
            }
            NumberOfTransactions = noTransactions.ToString();
            Expense = _expense.ToString();
            Income = _income.ToString();
        }

    }
}
