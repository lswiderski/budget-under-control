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
    public partial class AddTransaction : ContentPage
    {
        IAddTransactionViewModel vm;
        public AddTransaction()
        {
            using (var scope = App.Container.BeginLifetimeScope())
            {
                this.BindingContext = vm = scope.Resolve<IAddTransactionViewModel>();
            }
            InitializeComponent();
        }

        async void OnAddButtonClicked(object sender, EventArgs args)
        {
            vm.AddTransacion();
            await Navigation.PopModalAsync();
        }
    }
}