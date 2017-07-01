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
        ITransactionsViewModel vm;
        public Transactions()
        {
            InitializeComponent();
            using (var scope = App.Container.BeginLifetimeScope())
            {
                 this.BindingContext = vm = scope.Resolve<ITransactionsViewModel>();
            }

             
    }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            vm.LoadTransactions();
        }
    }
}