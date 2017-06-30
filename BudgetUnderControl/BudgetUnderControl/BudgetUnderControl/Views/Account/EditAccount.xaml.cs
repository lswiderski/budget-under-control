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
    public partial class EditAccount : ContentPage
    {
        IEditAccountViewModel vm;

        int accountId;
        public EditAccount(int accountId)
        {
            using (var scope = App.Container.BeginLifetimeScope())
            {
                this.BindingContext = vm = scope.Resolve<IEditAccountViewModel>();
            }
            this.accountId = accountId;
            InitializeComponent();
        }

        async void OnEditButtonClicked(object sender, EventArgs args)
        {
            vm.SaveAccount();
            await Navigation.PopModalAsync(true);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            vm.LoadAccount(accountId);
        }
    }
}