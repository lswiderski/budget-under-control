using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetUnderControl.CommonInfrastructure.Commands
{
    public class DeleteAccount : ICommand
    {
        public Guid Id { get; set; }
    }
}
