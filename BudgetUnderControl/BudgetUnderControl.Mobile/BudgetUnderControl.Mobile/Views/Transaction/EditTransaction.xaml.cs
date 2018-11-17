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
    public partial class EditTransaction : ContentPage
    {
        IEditTransactionViewModel vm;
        Guid transactionId;
        public EditTransaction(Guid transactionId)
        {
            using (var scope = App.Container.BeginLifetimeScope())
            {
                this.BindingContext = vm = scope.Resolve<IEditTransactionViewModel>();
            }
            this.transactionId = transactionId;
            InitializeComponent();
        }

        async void OnEditButtonClicked(object sender, EventArgs args)
        {
            await vm.EditTransactionAsync();
            await Navigation.PopModalAsync();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await vm.GetTransactionAsync(transactionId);
        }

        async void OnDeleteButtonClicked(object sender, EventArgs args)
        {
            var remove = await this.DisplayAlert("Remove", "Do you want to remove this Transaction?", "Yes", "No");
            if (!remove) return;
            await vm.DeleteTransactionAsync();
            await Navigation.PopModalAsync();
        }
    }
}