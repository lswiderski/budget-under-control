using BudgetUnderControl.Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetUnderControl.Infrastructure.Services
{
    public interface IUserIdentityContext
    {
        int UserId { get; }

        Guid ExternalId { get; }

        UserRole Role { get; }

        string RoleName { get; }
    }
}
