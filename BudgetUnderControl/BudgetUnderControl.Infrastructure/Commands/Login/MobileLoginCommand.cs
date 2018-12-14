using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetUnderControl.Infrastructure.Commands
{
    public class MobileLoginCommand : ICommand
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public Guid TokenId { get; set; }
    }
}
