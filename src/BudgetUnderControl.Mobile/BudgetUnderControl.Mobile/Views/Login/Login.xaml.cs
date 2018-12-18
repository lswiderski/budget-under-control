using Autofac;
using BudgetUnderControl.Mobile.ViewModels;
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
	public partial class Login : ContentPage
	{
        ILoginViewModel vm;
        public Login ()
		{
            using (var scope = App.Container.BeginLifetimeScope())
            {
                this.BindingContext = vm = scope.Resolve<ILoginViewModel>();
            }
            InitializeComponent(); 
        }

        protected override async void OnAppearing()
        {
            await vm.LogoutAsync();
        }

        async void OnLoginButtonClickedAsync(object sender, EventArgs args)
        {
            await vm.LoginAsync();

        }
    }
}