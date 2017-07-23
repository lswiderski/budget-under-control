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

        public AccountDetailsViewModel(IAccountModel accountModel, ITransactionModel transactionModel)
        {
            this.accountModel = accountModel;
            this.transactionModel = transactionModel;
        }

        public async void LoadAccount(int accountId)
        {
            this.accountId = accountId;
            var account = await this.accountModel.GetAccount(accountId);
            Name = account.Name;
            ValueWithCurrency = account.AmountWithCurrency;
            Value = account.Amount;
        }
        public async void LoadTransactions(int accountId)
        {
            Transactions = await transactionModel.GetTransactions(accountId);
        }

        public void RemoveAccount()
        {
            accountModel.RemoveAccount(accountId);
        }
    }
}
