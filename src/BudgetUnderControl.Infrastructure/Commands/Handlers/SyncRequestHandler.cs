using BudgetUnderControl.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BudgetUnderControl.CommonInfrastructure.Commands
{
    public class SyncRequestHandler : ICommandHandler<SyncRequest>
    {
        private readonly ISyncService syncService;
        public SyncRequestHandler(ISyncService syncService)
        {
            this.syncService = syncService;
        }

        public async Task HandleAsync(SyncRequest command)
        {
            await this.syncService.SyncAsync(command);
        }
    }
}
