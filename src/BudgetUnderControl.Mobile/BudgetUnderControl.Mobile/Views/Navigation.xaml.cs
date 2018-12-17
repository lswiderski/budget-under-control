using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using BudgetUnderControl.Mobile.Keys;
using Autofac;
using BudgetUnderControl.Mobile;

namespace BudgetUnderControl.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Navigation : ContentPage
    {

        INavigationViewModel vm;
        public Navigation()
        {
            using (var scope = App.Container.BeginLifetimeScope())
            {
                this.BindingContext = vm = scope.Resolve<INavigationViewModel>();
            }

            InitializeComponent();
        }

        public ListView ListView { get { return listView; } }

        async void OnLoginButtonClicked(object sender, EventArgs args)
        {
             App.MasterPage.NavigateTo(typeof(Login));
        }

        async void OnLogoutButtonClicked(object sender, EventArgs args)
        {
            App.MasterPage.NavigateTo(typeof(Logout));
        }
    }
}