using BudgetUnderControl.CommonInfrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BudgetUnderControl.CommonInfrastructure
{
    public interface IUserService
    {
        IUserIdentityContext CreateUserIdentityContext();
    }
}
