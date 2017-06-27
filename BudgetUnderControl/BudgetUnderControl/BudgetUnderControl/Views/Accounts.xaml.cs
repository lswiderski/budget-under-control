using Autofac;
using BudgetUnderControl.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BudgetUnderControl.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Accounts : ContentPage
    {
        private readonly FloatingActionButtonView fab;
        private readonly ListView list;
        private int appearingListItemIndex = 0;

        IAccountModel accountModel;
        public Accounts()
        {
            InitializeComponent();
            using (var scope = App.Container.BeginLifetimeScope())
            {
                this.accountModel = scope.Resolve<IAccountModel>();
            }


            fab = new FloatingActionButtonView()
            {
                ImageName = "ic_add.png",
                ColorNormal = Color.FromHex("ff3498db"),
                ColorPressed = Color.Black,
                ColorRipple = Color.FromHex("ff3498db"),
                Clicked = async (sender, args) =>
                {
                    var animate = await this.DisplayAlert("Fab", "Hide and show the Fab?", "Sure", "Not now");
                    if (!animate) return;

                    fab.Hide();
                    await Task.Delay(1500);
                    fab.Show();
                },
            };

            // Overlay the FAB in the bottom-right corner
            AbsoluteLayout.SetLayoutFlags(fab, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(fab, new Rectangle(1f, 1f, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
            absoluteLayout.Children.Add(fab);
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            var model = accountModel.GetAccounts();

            accounts.ItemsSource = await model;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }

    }
}