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

        ISyncMobileService syncService;
        public SettingsViewModel(ISyncMobileService syncService)
        {
            this.syncService = syncService;
        }

        public async Task ExportBackupAsync()
        {
            await syncService.SaveBackupFileAsync();
        }

        public async Task ImportBackupAsync()
        {
            await syncService.LoadBackupFileAsync();
        }

        public async Task ExportCSVAsync()
        {
            await syncService.ExportCSVAsync();
        }

        public async Task ExportDBAsync()
        {
            await syncService.ExportDBAsync();
        }
    }
}
