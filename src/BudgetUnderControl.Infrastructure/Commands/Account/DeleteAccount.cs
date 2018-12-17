using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetUnderControl.Infrastructure.Commands
{
    public class DeleteAccount : ICommand
    {
        public Guid Id { get; set; }
    }
}
