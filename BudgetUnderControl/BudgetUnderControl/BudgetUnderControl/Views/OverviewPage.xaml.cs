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
            pln.Text = dict.ContainsKey("PLN") ? dict["PLN"].ToString() : "0";
            usd.Text = dict.ContainsKey("USD") ? dict["USD"].ToString() : "0";
            eur.Text = dict.ContainsKey("EUR") ? dict["EUR"].ToString() : "0";

            total.Text = ((dict.ContainsKey("PLN") ? dict["PLN"] : 0)
                + ((dict.ContainsKey("USD") ? dict["USD"] : 0) * 3.54m)
                + ((dict.ContainsKey("EUR") ? dict["EUR"] : 0) * 4.20m)).ToString();
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