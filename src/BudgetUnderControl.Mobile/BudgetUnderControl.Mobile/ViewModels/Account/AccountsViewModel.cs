using BudgetUnderControl.Common.Contracts;
using BudgetUnderControl.CommonInfrastructure.Commands;
using BudgetUnderControl.Mobile.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetUnderControl.ViewModel
{
    public class AccountsViewModel : IAccountsViewModel, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        IAccountService accountService;

        ICollection<AccountListItemDTO> accounts;
        public ICollection<AccountListItemDTO> Accounts
        {
            get
            {
                return accounts;
            }
            set
            {
                if (accounts != value)
                {
                    accounts = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Accounts)));
                }

            }
        }

        AccountListItemDTO selectedAccount;
        public AccountListItemDTO SelectedAccount
        {
            get
            {
                return selectedAccount;
            }
            set
            {
                if (selectedAccount != value)
                {
                    selectedAccount = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedAccount)));
                }

            }
        }

        public AccountsViewModel(IAccountService accountService)
        {
            this.accountService = accountService;
        }

        public async Task LoadAccounts()
        {
            Accounts = await accountService.GetAccountsWithBalanceAsync();
        }
    }
}
