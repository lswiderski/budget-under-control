using BudgetUnderControl.Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetUnderControl.Infrastructure.Commands
{
    public class SyncRequest : ICommand
    {
        public DateTime LastSync { get; set; }

        public Guid UserId { get; set; }
        public SynchronizationComponent Component { get; set; }
        public Guid ComponentId { get; set; }

       //add collections TransactionsToUpdate / TransfersToUpdate / CategoriesToUpdate etc
    }
}
