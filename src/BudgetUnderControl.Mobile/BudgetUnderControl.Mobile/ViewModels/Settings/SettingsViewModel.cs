using Autofac;
using BudgetUnderControl.Infrastructure;
using BudgetUnderControl.Infrastructure.Services;
using BudgetUnderControl.Infrastructure.Settings;
using BudgetUnderControl.Mobile.Keys;
using BudgetUnderControl.Mobile.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace BudgetUnderControl.ViewModel
{
    public class SettingsViewModel : ISettingsViewModel, INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        private string apiUrl;
        public string ApiUrl
        {
            get => apiUrl;
            set
            {
                if (apiUrl != value)
                {
                    apiUrl = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ApiUrl)));
                }
            }
        }

        private bool isLoading;
        public bool IsLoading
        {
            get => isLoading;
            set
            {
                if (isLoading != value)
                {
                    isLoading = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsLoading)));
                }
            }
        }

        ISyncMobileService syncMobileService;
        private readonly GeneralSettings settings;

        public SettingsViewModel(ISyncMobileService syncMobileService, GeneralSettings settings)
        {
            this.syncMobileService = syncMobileService;
            this.settings = settings;
            var url = Preferences.Get(PreferencesKeys.APIURL, string.Empty);
            apiUrl = string.IsNullOrEmpty(url) || string.IsNullOrWhiteSpace(url) ? settings.ApiBaseUri : url;

        }

        public async Task ExportBackupAsync()
        {
            IsLoading = true;
            await syncMobileService.SaveBackupFileAsync();
            IsLoading = false;
        }

        public async Task ImportBackupAsync()
        {
            await syncMobileService.LoadBackupFileAsync();
        }


        public async Task ExportCSVAsync()
        {
            IsLoading = true;
            await syncMobileService.ExportCSVAsync();
            IsLoading = false;
        }

        public async Task ExportDBAsync()
        {
            IsLoading = true;
            await syncMobileService.ExportDBAsync();
            IsLoading = false;
        }

        public async Task ClearSyncDB()
        {
            IsLoading = true;
            await syncMobileService.TaskClearSyncDB();
            IsLoading = false;
        }

        public async Task ClearLocalData()
        {
            IsLoading = true;
            await syncMobileService.CleanDataBaseAsync();
            IsLoading = false;
        }

        public async Task SyncAsync()
        {
            IsLoading = true;
            await syncMobileService.SyncAsync();
            IsLoading = false;
        }

        public void OnApiUrlChange()
        {
            Preferences.Set(PreferencesKeys.APIURL, ApiUrl);
            var httpClient =  App.Container.ResolveNamed<HttpClient>("api");
            httpClient.BaseAddress = new Uri(ApiUrl);
        }

        public bool IsLogged
        {
            get => Preferences.Get(PreferencesKeys.IsUserLogged, false);
            set
            {
                if (Preferences.Get(PreferencesKeys.IsUserLogged, false) != value)
                {
                    Preferences.Set(PreferencesKeys.IsUserLogged, value);
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsLogged)));
                }
            }
        }

       

    }
}
