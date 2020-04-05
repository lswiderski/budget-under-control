using BudgetUnderControl.CommonInfrastructure.Commands;
using System.Threading.Tasks;

namespace BudgetUnderControl.Infrastructure.Services
{
    public interface ISynchroniser
    {
        Task SynchroniseAsync(SyncRequest syncRequest);
    }
}
