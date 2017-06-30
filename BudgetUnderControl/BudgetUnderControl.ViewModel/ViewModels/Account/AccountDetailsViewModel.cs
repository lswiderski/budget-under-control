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

        public AccountDetailsViewModel(IAccountModel accountModel)
        {
            this.accountModel = accountModel;
        }

        public async void LoadAccount(int accountId)
        {
            this.accountId = accountId;
            var account = await this.accountModel.GetAccount(accountId);
            Name = account.Name;
        }

        public void RemoveAccount()
        {
            accountModel.RemoveAccount(accountId);
        }
    }
}
