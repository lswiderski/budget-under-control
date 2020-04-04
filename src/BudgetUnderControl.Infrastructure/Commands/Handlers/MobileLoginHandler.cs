using BudgetUnderControl.Infrastructure.Services;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BudgetUnderControl.CommonInfrastructure.Commands.Login
{
    public class MobileLoginHandler : ICommandHandler<MobileLoginCommand>
    {
        private readonly ILoginService loginService;
        public MobileLoginHandler(ILoginService loginService)
        {
            this.loginService = loginService;
        }

        public async Task HandleAsync(MobileLoginCommand command)
        {
            await this.loginService.ValidateLoginAsync(command);
        }
    }
}
