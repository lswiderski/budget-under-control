using BudgetUnderControl.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetUnderControl.ViewModel
{
    public class AccountDetailsViewModel : IAccountDetailsViewModel, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        IAccountModel accountModel;
        ITransactionModel transactionModel;

        int accountId;

        private string name;
        public string Name
        {
            get => name;
            set
            {
                if (name != value)
                {
                    name = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));
                }
            }
        }

        private string valueWithCurrency;
        public string ValueWithCurrency
        {
            get => valueWithCurrency;
            set
            {
                if (valueWithCurrency != value)
                {
                    valueWithCurrency = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ValueWithCurrency)));
                }
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

        private decimal _value;
        public decimal Value
        {
            get => _value;
            set
            {
                if (_value != value)
                {
                    _value = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ValueWithCurrency)));
                }
            }
        }

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

        public AccountDetailsViewModel(IAccountModel accountModel, ITransactionModel transactionModel)
        {
            this.accountModel = accountModel;
            this.transactionModel = transactionModel;

            var now = DateTime.UtcNow;
            FromDate = new DateTime(now.Year, now.Month, 1);
            ToDate = new DateTime(now.Year, now.Month, DateTime.DaysInMonth(now.Year, now.Month));
        }

        public async void LoadAccount(int accountId)
        {
            this.accountId = accountId;
            var account = await this.accountModel.GetAccountDetails(accountId, FromDate, ToDate);
            Name = "Account: " + account.Name;
            ValueWithCurrency = account.AmountWithCurrency;
            Value = account.Amount;
            Expense = account.Expense.ToString();
            Income = account.Income.ToString();
        }
        public async void LoadTransactions(int accountId)
        {
            Transactions = await transactionModel.GetTransactions(accountId, FromDate, ToDate);

            var account = await this.accountModel.GetAccountDetails(accountId, FromDate, ToDate);
            Expense = account.Expense.ToString();
            Income = account.Income.ToString();
        }

        public void RemoveAccount()
        {
            accountModel.RemoveAccount(accountId);
        }

        public void SetNextMonth()
        {
            FromDate = new DateTime(FromDate.Year, FromDate.Month, 1).AddMonths(1);
            ToDate = new DateTime(FromDate.Year, FromDate.Month, DateTime.DaysInMonth(FromDate.Year, FromDate.Month));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ActualRange)));
            LoadTransactions(accountId);
        }

        public void SetPreviousMonth()
        {
            FromDate = new DateTime(FromDate.Year, FromDate.Month, 1).AddMonths(-1);
            ToDate = new DateTime(FromDate.Year, FromDate.Month, DateTime.DaysInMonth(FromDate.Year, FromDate.Month));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ActualRange)));
            LoadTransactions(accountId);
        }
    }
}
