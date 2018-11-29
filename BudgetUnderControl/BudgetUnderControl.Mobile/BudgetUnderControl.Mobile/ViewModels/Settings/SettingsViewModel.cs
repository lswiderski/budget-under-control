using BudgetUnderControl.Infrastructure;
using BudgetUnderControl.Infrastructure.Services;
using BudgetUnderControl.Mobile.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetUnderControl.ViewModel
{
    public class SettingsViewModel : ISettingsViewModel
    {

        ISyncMobileService syncMobileService;
        public SettingsViewModel(ISyncMobileService syncMobileService)
        {
            this.syncMobileService = syncMobileService;
        }

        public async Task ExportBackupAsync()
        {
            await syncMobileService.SaveBackupFileAsync();
        }

        public async Task ImportBackupAsync()
        {
            await syncMobileService.LoadBackupFileAsync();
        }

        public async Task ExportCSVAsync()
        {
            await syncMobileService.ExportCSVAsync();
        }

        public async Task ExportDBAsync()
        {
            await syncMobileService.ExportDBAsync();
        }

        public async Task ClearSyncDB()
        {
            await syncMobileService.TaskClearSyncDB();
        }

        public async Task SyncAsync()
        {
            await syncMobileService.SyncAsync();
        }
    }
}
