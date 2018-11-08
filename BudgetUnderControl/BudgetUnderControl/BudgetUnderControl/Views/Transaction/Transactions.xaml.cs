using Autofac;
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
    public partial class Transactions : ContentPage
    {
        private FloatingActionButtonView fab;

        ITransactionsViewModel vm;
        public Transactions()
        {
            InitializeComponent();
            using (var scope = App.Container.BeginLifetimeScope())
            {
                 this.BindingContext = vm = scope.Resolve<ITransactionsViewModel>();
            }

            InitFAB();
    }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await vm.LoadTransactionsAsync();
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

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var transactionId = vm.SelectedTransaction.Id;
            //App.MasterPage.NavigateTo(typeof(EditTransaction), transactionId);
            var editTransaction = new EditTransaction(transactionId);
            await Navigation.PushModalAsync(editTransaction);
        }

        protected async Task OnNextMonthButtonClicked(object sender, EventArgs args)
        {
             await vm.SetNextMonth();
        }

        protected async Task OnPreviousMonthButtonClicked(object sender, EventArgs args)
        {
            await vm.SetPreviousMonth();
        }
    }
}