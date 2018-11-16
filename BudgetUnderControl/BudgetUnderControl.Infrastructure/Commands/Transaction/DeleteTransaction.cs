using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetUnderControl.Infrastructure.Commands
{
    public class DeleteTransaction : ICommand
    {
        public int? Id { get; set; }

        public Guid? ExternalId { get; set; }
    }
}
