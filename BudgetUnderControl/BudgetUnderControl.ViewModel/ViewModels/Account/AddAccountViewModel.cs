using BudgetUnderControl.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetUnderControl.ViewModel
{
    public class AddAccountViewModel : IAddAccountViewModel, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        IAccountModel accountModel;
        ICurrencyModel currencyModel;
        IAccountGroupModel accountGroupModel;

        int accountId;

        List<CurrencyDTO> currencies;
        List<AccountGroupItemDTO> accountGroups;
        public List<AccountGroupItemDTO> AccountGroups => accountGroups;
        public List<CurrencyDTO> Currencies => currencies;

        int selectedCurrencyIndex;
        public int SelectedCurrencyIndex
        {
            get
            {
                return selectedCurrencyIndex;
            }
            set
            {
                if (selectedCurrencyIndex != value)
                {
                    selectedCurrencyIndex = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedCurrencyIndex)));
                }

            }
        }

        int selectedAccountGroupIndex;

        public int SelectedAccountGroupIndex
        {
            get
            {
                return selectedAccountGroupIndex;
            }
            set
            {
                if (selectedAccountGroupIndex != value)
                {
                    selectedAccountGroupIndex = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedAccountGroupIndex)));
                }

            }
        }


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

        private string comment;
        public string Comment
        {
            get => comment;
            set
            {
                if (comment != value)
                {
                    comment = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Comment)));
                }
            }
        }

        private bool isInTotal;
        public bool IsInTotal
        {
            get => isInTotal;
            set
            {
                if (isInTotal != value)
                {
                    isInTotal = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsInTotal)));
                }
            }
        }

        private string amount;
        public string Amount
        {
            get => amount;
            set
            {
                if (amount != value)
                {
                    amount = value;
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Amount)));
                }
            }
        }

        public AddAccountViewModel(IAccountModel accountModel, ICurrencyModel currencyModel, IAccountGroupModel accountGroupModel)
        {
            this.accountModel = accountModel;
            this.currencyModel = currencyModel;
            this.accountGroupModel = accountGroupModel;
            GetDropdowns();
        }

        async void GetDropdowns()
        {
            currencies = (await currencyModel.GetCurriences()).ToList();
            accountGroups = (await accountGroupModel.GetAccountGroups()).ToList();
        }

        public void AddAccount()
        {
            decimal value;
            decimal.TryParse(amount.Replace(',', '.'), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out value);
            var dto = new AddAccountDTO
            {
                Name = Name,
                Comment = Comment,
                Amount = value,
                AccountGroupId = AccountGroups[SelectedAccountGroupIndex].Id,
                CurrencyId = Currencies[SelectedCurrencyIndex].Id,
                IsIncludedInTotal = IsInTotal,
            };

            accountModel.AddAccount(dto);
        }
    }
}
