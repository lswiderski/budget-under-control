using BudgetUnderControl.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetUnderControl.Infrastructure.Services
{
    public interface ISyncService
    {
        Task ImportBackUpAsync(BackUpDTO backupDto);
        Task<BackUpDTO> GetBackUpAsync();
        Task<IEnumerable<string>> GenerateCSV();
    }
}
