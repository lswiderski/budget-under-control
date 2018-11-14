using BudgetUnderControl.Common.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetUnderControl.Common.Enums
{
    public enum UserRole : int
    {
        [StringValue("User")]
        User = 1,

        [StringValue("Admin")]
        Admin = 2
    }
}
