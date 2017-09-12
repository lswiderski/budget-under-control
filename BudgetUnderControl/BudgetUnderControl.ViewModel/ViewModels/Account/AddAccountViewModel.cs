using BudgetUnderControl.Common.Enums;
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
        
        List<AccountTypeDTO> accountTypes;
        public List<AccountTypeDTO> AccountTypes => accountTypes;
        List<AccountListItemDTO> accounts;
        public List<AccountListItemDTO> Accounts => accounts;

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

        int selectedAccountTypeIndex;
        public int SelectedAccountTypeIndex
        {
            get
            {
                return selectedAccountTypeIndex;
            }
            set
            {
                if (selectedAccountTypeIndex != value)
                {
                    selectedAccountTypeIndex = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedAccountTypeIndex)));
                }

            }
        }

        int selectedAccountIndex;
        public int SelectedAccountIndex
        {
            get
            {
                return selectedAccountIndex;
            }
            set
            {
                if (selectedAccountIndex != value)
                {
                    selectedAccountIndex = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedAccountIndex)));
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

        private string order;
        public string Order
        {
            get => order;
            set
            {
                if (order != value)
                {
                    order = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Order)));
                }
            }
        }

        public AddAccountViewModel(IAccountModel accountModel, ICurrencyModel currencyModel, IAccountGroupModel accountGroupModel)
        {
            this.accountModel = accountModel;
            this.currencyModel = currencyModel;
            this.accountGroupModel = accountGroupModel;
            GetDropdowns();

            selectedAccountIndex = -1;
        }

        async void GetDropdowns()
        {
            currencies = (await currencyModel.GetCurriences()).ToList();
            accountGroups = (await accountGroupModel.GetAccountGroups()).ToList();
            accounts = accountModel.GetAccounts().ToList();
            accountTypes = this.GetAccountTypes().ToList();
           
        }

        public void AddAccount()
        {
            decimal value;
            int _order = 0;
            if(!int.TryParse(order, out _order))
            {
                _order = 0;
            }
            decimal.TryParse(amount.Replace(',', '.'), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out value);
            var dto = new AddAccountDTO
            {
                Name = Name,
                Comment = Comment,
                Amount = value,
                Order = _order,
                AccountGroupId = AccountGroups[SelectedAccountGroupIndex].Id,
                CurrencyId = Currencies[SelectedCurrencyIndex].Id,
                IsIncludedInTotal = IsInTotal,
                Type = (AccountType)AccountTypes[SelectedAccountTypeIndex].Id,
                ParentAccountId = selectedAccountIndex > -1 ? Accounts[SelectedAccountIndex].Id : (int?)null,
            };

            accountModel.AddAccount(dto);
        }

        public void ClearParentAccountCombo()
        {
            SelectedAccountIndex = -1;
        }

        private ICollection<AccountTypeDTO> GetAccountTypes()
        {
            var collection = new List<AccountTypeDTO>();
            collection.Add(new AccountTypeDTO { Id = (int)AccountType.Account, Name = AccountType.Account.ToString() });
            collection.Add(new AccountTypeDTO { Id = (int)AccountType.Wallet, Name = AccountType.Wallet.ToString() });
            collection.Add(new AccountTypeDTO { Id = (int)AccountType.Card, Name = AccountType.Card.ToString() });
            return collection;
        }
    }
}
