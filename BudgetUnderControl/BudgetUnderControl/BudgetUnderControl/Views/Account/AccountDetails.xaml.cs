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
        private FloatingActionButtonView fab;
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

            InitFAB();
        }

        private void InitFAB()
        {
            fab = new FloatingActionButtonView()
            {
                ImageName = "ic_add.png",
                ColorNormal = Color.FromHex("ff3498db"),
                ColorPressed = Color.Black,
                ColorRipple = Color.FromHex("ff3498db"),
                Clicked = OnAddButtonClicked
            };

            // Overlay the FAB in the bottom-right corner
            AbsoluteLayout.SetLayoutFlags(fab, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(fab, new Rectangle(1f, 1f, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
            absoluteLayout.Children.Add(fab);
        }

        protected async void OnAddButtonClicked(object sender, EventArgs args)
        {
            var addAccount = new AddTransaction();
            await Navigation.PushModalAsync(addAccount);
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

            await vm.RemoveAccount();
            App.MasterPage.NavigateTo(typeof(Accounts));
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await vm.LoadAccount(accountId);
            await vm.LoadTransactions(accountId);
        }

        private void valueLabel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            valueLabel.TextColor = vm.Value < 0 ? Color.Red : Color.Green;
        }

        protected async Task OnNextMonthButtonClicked(object sender, EventArgs args)
        {
            await vm.SetNextMonth();
        }

        protected async Task OnPreviousMonthButtonClicked(object sender, EventArgs args)
        {
            await vm.SetPreviousMonth();
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var transactionId = vm.SelectedTransaction.Id;
            var editTransaction = new EditTransaction(transactionId);
            await Navigation.PushModalAsync(editTransaction);
        }
    }
}