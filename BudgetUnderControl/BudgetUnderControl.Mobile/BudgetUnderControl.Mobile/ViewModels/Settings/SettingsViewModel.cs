using BudgetUnderControl.Model;
using BudgetUnderControl.Model.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetUnderControl.ViewModel
{
    public class SettingsViewModel : ISettingsViewModel
    {

        ISyncService syncService;
        public SettingsViewModel(ISyncService syncService)
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

    }
}
