using Autofac;
using BudgetUnderControl.Domain.Repositiories;
using BudgetUnderControl.Model;
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
    public partial class Currencies : ContentPage
    {
        ICurrencyRepository currencyModel;
        public Currencies()
        {
            InitializeComponent();

            using (var scope = App.Container.BeginLifetimeScope())
            {
                this.currencyModel = scope.Resolve<ICurrencyRepository>();
            }
               
        }

        async void OnButtonClicked(object sender, EventArgs e)
        {
            var model = currencyModel.GetCurriences();

            curriences.ItemsSource = await model;
        }

        protected override async void OnAppearing()
        {
            var model = currencyModel.GetCurriences();

            curriences.ItemsSource = await model;
        }
    }
}