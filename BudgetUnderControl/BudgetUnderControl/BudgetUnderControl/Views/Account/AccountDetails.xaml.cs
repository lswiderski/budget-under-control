using Autofac;
using BudgetUnderControl.Model;
using BudgetUnderControl.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BudgetUnderControl.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AccountDetails : ContentPage
    {
        IAccountDetailsViewModel vm;
        int accountId;

        public AccountDetails(int accountId)
        {
            using (var scope = App.Container.BeginLifetimeScope())
            {
                this.BindingContext = vm = scope.Resolve<IAccountDetailsViewModel>();
            }
            this.accountId = accountId;

            InitializeComponent();
        }

        private async void Button_Edit_Clicked(object sender, EventArgs e)
        {
            var editAccount = new EditAccount(accountId);
            await Navigation.PushModalAsync(editAccount);
        }

        private async void Button_Remove_Clicked(object sender, EventArgs e)
        {
            var remove = await this.DisplayAlert("Remove", "Do you want to remove this account?", "Yes", "No");
            if (!remove) return;

            vm.RemoveAccount();
            App.MasterPage.NavigateTo(typeof(Accounts));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            vm.LoadAccount(accountId);
            vm.LoadTransactions(accountId);
        }

        private void valueLabel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            valueLabel.TextColor = vm.Value < 0 ? Color.Red : Color.Green;
        }
    }
}