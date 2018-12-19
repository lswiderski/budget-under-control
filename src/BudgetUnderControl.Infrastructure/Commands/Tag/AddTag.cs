using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetUnderControl.Infrastructure.Commands
{
    public class AddTag : ICommand
    {
        public string Name { get; set; }

        public Guid ExternalId { get; set; }

        public AddTag()
        {
            this.ExternalId = Guid.NewGuid();
        }
    }
}
