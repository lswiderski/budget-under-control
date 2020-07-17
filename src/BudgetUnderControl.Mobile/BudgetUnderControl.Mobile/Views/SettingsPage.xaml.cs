using Autofac;
using BudgetUnderControl.Common;
using BudgetUnderControl.Common.Contracts.System;
using BudgetUnderControl.Common.Enums;
using BudgetUnderControl.Mobile.PlatformSpecific;
using BudgetUnderControl.ViewModel;
using Plugin.LocalNotifications;
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
    public partial class SettingsPage : ContentPage
    {
        ISettingsViewModel vm;
        public SettingsPage()
        {
            using (var scope = App.Container.BeginLifetimeScope())
            {
                this.BindingContext = vm = scope.Resolve<ISettingsViewModel>();
            }

            InitializeComponent();
            apiUrl.Unfocused += (object sender, FocusEventArgs e) => { vm.OnApiUrlChange(); };
        }

        private async void ExportButton_Clicked(object sender, EventArgs e)
        {
            await vm.ExportBackupAsync();
        }

        private async void ImportButton_Clicked(object sender, EventArgs e)
        {
            vm.IsLoading = true;

            vm.ImportBackupAsync().Wait();
            vm.IsLoading = false;
        }

        private async void ExportCSVButton_Clicked(object sender, EventArgs e)
        {
            await vm.ExportCSVAsync();
        }

        private async void ExportSQLDBButton_Clicked(object sender, EventArgs e)
        {
            await vm.ExportDBAsync();
        }

        private async void SyncButton_Clicked(object sender, EventArgs e)
        {
            await vm.SyncAsync();
        }

        private async void ClearSyncDBButton_Clicked(object sender, EventArgs e)
        {
            await vm.ClearSyncDB();
        }

        private async void ClearLocalDataButton_Clicked(object sender, EventArgs e)
        {
            await vm.ClearLocalData();
        }

        async void OnLoginButtonClicked(object sender, EventArgs args)
        {
            App.MasterPage.NavigateTo("login");
        }

        async void OnLogoutButtonClicked(object sender, EventArgs args)
        {
            App.MasterPage.NavigateTo("logout");
        }


    }
}