using BudgetUnderControl.Common.Enums;
using BudgetUnderControl.Infrastructure.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BudgetUnderControl.Infrastructure.Services
{
    public interface ISyncRequestBuilder
    {
        Task<SyncRequest> CreateSyncRequestAsync(SynchronizationComponent source, SynchronizationComponent target);
    }
}
