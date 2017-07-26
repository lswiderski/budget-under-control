using BudgetUnderControl.Model;
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

        public void ExportBackup()
        {
            
            syncService.SaveBackupFile();
        }

        public void ImportBackup()
        {

            syncService.LoadBackupFile();
        }
        public void ExportCSV()
        {

            syncService.ExportCSV();
        }

    }
}
