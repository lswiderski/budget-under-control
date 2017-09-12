using BudgetUnderControl.Model;
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
        IAccountModel accountModel;

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

        public AccountsViewModel(IAccountModel accountModel)
        {
            this.accountModel = accountModel;
        }

        public async void LoadAccounts()
        {
            Accounts = accountModel.GetAccounts();
        }
    }
}
