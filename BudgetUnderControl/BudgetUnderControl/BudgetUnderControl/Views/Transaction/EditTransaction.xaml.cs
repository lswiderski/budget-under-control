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
        int transactionId;
        public EditTransaction(int transactionId)
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
            vm.EditTransaction();
            await Navigation.PopModalAsync();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            vm.GetTransaction(transactionId);
        }

        async void OnDeleteButtonClicked(object sender, EventArgs args)
        {
            var remove = await this.DisplayAlert("Remove", "Do you want to remove this Transaction?", "Yes", "No");
            if (!remove) return;
            vm.DeleteTransaction();
            await Navigation.PopModalAsync();
        }
    }
}