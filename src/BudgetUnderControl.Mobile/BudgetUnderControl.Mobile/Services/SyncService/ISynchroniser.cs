using BudgetUnderControl.CommonInfrastructure.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BudgetUnderControl.Mobile.Services
{
    public interface ISynchroniser
    {
        Task SynchroniseAsync(SyncRequest syncRequest);
    }
}
