using Autofac;
using BudgetUnderControl.Common.Enums;
using BudgetUnderControl.Common.Contracts;
using BudgetUnderControl.Infrastructure;
using BudgetUnderControl.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BudgetUnderControl.Infrastructure.Commands;

namespace BudgetUnderControl.ViewModel
{
    public class EditAccountViewModel : IEditAccountViewModel, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        IAccountService accountService;
        ICurrencyService currencyService;
        IAccountGroupService accountGroupService;
        ICommandDispatcher commandDispatcher;

        int accountId;
        Guid ExternalId;

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
                    selectedAccountGroupIndex  = value;
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

        private bool isActive;
        public bool IsActive
        {
            get => isActive;
            set
            {
                if (isActive != value)
                {
                    isActive = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsActive)));
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

        public EditAccountViewModel(ICurrencyService currencyService, IAccountGroupService accountGroupService, IAccountService accountService, ICommandDispatcher commandDispatcher)
        {
            this.currencyService = currencyService;
            this.accountGroupService = accountGroupService;
            this.accountService = accountService;
            this.commandDispatcher = commandDispatcher;
            GetDropdowns();
        }

        async void GetDropdowns()
        {
            currencies = (await currencyService.GetCurriencesAsync()).ToList();
            accountGroups = (await accountGroupService.GetAccountGroupsAsync()).ToList();
            accounts = (await accountService.GetAccountsWithBalanceAsync()).ToList();
            accountTypes = this.GetAccountTypes().ToList();
        }

        public async void LoadAccount(Guid accountId)
        {
            var account = await this.accountService.GetAccountAsync(accountId);
            Amount = account.Amount.ToString();
            Name = account.Name;
            Comment = account.Comment;
            IsInTotal = account.IsIncludedInTotal;
            IsActive = account.IsActive;
            SelectedAccountGroupIndex = AccountGroups.IndexOf(AccountGroups.FirstOrDefault(y => y.Id == account.AccountGroupId));
            SelectedCurrencyIndex = Currencies.IndexOf(Currencies.FirstOrDefault(y => y.Id == account.CurrencyId));
            SelectedAccountIndex = account.ParentAccountId.HasValue ? Accounts.IndexOf(Accounts.FirstOrDefault(y => y.Id == account.ParentAccountId)) : -1;
            SelectedAccountTypeIndex = AccountTypes.IndexOf(AccountTypes.FirstOrDefault(y => y.Id == (int)account.Type));
            Order = account.Order.ToString();
            this.ExternalId = accountId;
            this.accountId = account.Id;
        }

        public async Task SaveAccount()
        {
            decimal value;
            decimal.TryParse(amount.Replace(',', '.'), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out value);
            int _order = 0;
            int.TryParse(order, out _order);

            var command = new EditAccount
            {
                Name = Name,
                Comment = Comment,
                Amount = value,
                Order = _order,
                AccountGroupId = AccountGroups[SelectedAccountGroupIndex].Id,
                CurrencyId = Currencies[SelectedCurrencyIndex].Id,
                IsIncludedInTotal = IsInTotal,
                Id = accountId,
                IsActive = IsActive,
                Type = (AccountType)AccountTypes[SelectedAccountTypeIndex].Id,
                ParentAccountId = selectedAccountIndex > -1 ? Accounts[SelectedAccountIndex].Id : (int?)null,
                ExternalId = ExternalId
            };
           
            using (var scope = App.Container.BeginLifetimeScope())
            {
                await commandDispatcher.DispatchAsync(command, scope);
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
