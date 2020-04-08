using BudgetUnderControl.CommonInfrastructure.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BudgetUnderControl.CommonInfrastructure
{
    public interface ILoginService
    {
        Task ValidateLoginAsync(MobileLoginCommand command);
    }
}
