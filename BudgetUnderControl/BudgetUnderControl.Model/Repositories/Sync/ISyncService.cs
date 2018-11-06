using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetUnderControl.Model
{
    public interface ISyncService
    {
        string GetTransactionsJSON();
        void ImportBackUpJSON(string json);
        string GetBackUpJSON();
        void SaveBackupFile();
        void LoadBackupFile();
        void ExportCSV();
    }
}
