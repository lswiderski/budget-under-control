using BudgetUnderControl.Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BudgetUnderControl.MobileDomain.Repositiories
{
    public interface ISynchronizationRepository
    {
        Task<IEnumerable<Synchronization>> GetSynchronizationsAsync();
        Task AddSynchronizationAsync(Synchronization synchronization);
        Task<Synchronization> GetSynchronizationAsync(SynchronizationComponent component, string componentId, int userId);
        Task UpdateAsync(Synchronization synchronization);
        void Update(Synchronization synchronization);
        Task ClearSynchronizationAsync();
    }
}
