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
    public partial class OverviewPage : ContentPage
    {
        private FloatingActionButtonView fab;
        IOverviewViewModel vm;
        public OverviewPage()
        {
            using (var scope = App.Container.BeginLifetimeScope())
            {
                this.BindingContext = vm = scope.Resolve<IOverviewViewModel>();
            }

            InitializeComponent();
            InitFAB();

        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            var dict = await vm.GetTotalsAsync();
            string plnCode = "PLN";
            string eurCode = "EUR";
            string usdCode = "USD";
            string format = "0.##";
            string defaultValue = "0";
            pln.Text = dict.ContainsKey(plnCode) ? dict[plnCode].ToString(format) : defaultValue;
            usd.Text = dict.ContainsKey(usdCode) ? dict[usdCode].ToString(format) : defaultValue;
            eur.Text = dict.ContainsKey(eurCode) ? dict[eurCode].ToString(format) : defaultValue;

            total.Text = ((dict.ContainsKey(plnCode) ? dict[plnCode] : 0)
                + (await vm.CalculateValueAsync(dict.ContainsKey(usdCode) ? dict[usdCode] : 0, usdCode, plnCode))
                + (await vm.CalculateValueAsync(dict.ContainsKey(eurCode) ? dict[eurCode] : 0, eurCode, plnCode))).ToString("0.##");
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

    }
}