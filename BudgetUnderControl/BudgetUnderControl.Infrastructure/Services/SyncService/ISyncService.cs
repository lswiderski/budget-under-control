using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetUnderControl.Model.Services
{
    public interface ISyncService
    {
        Task<string> GetTransactionsJSONAsync();
        Task ImportBackUpJSONAsync(string json);
        Task<string> GetBackUpJSONAsync();
        Task SaveBackupFileAsync();
        Task LoadBackupFileAsync();
        Task ExportCSVAsync();
        Task ExportDBAsync();
    }
}
