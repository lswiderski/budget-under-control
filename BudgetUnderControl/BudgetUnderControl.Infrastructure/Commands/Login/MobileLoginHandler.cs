using BudgetUnderControl.Infrastructure.Services;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BudgetUnderControl.Infrastructure.Commands.Login
{
    public class MobileLoginHandler : ICommandHandler<MobileLoginCommand>
    {
        private readonly ILoginService loginService;
        private readonly IMemoryCache cache;
        public MobileLoginHandler(ILoginService loginService, IMemoryCache cache)
        {
            this.loginService = loginService;
            this.cache = cache;
        }

        public async Task HandleAsync(MobileLoginCommand command)
        {
            var result = await this.loginService.ValidateLoginAsync(command);

            if(result)
            {
                var userId = await this.loginService.GetUserId(command.Username);
                cache.Set(command.TokenId, userId);
            }

        }
    }
}
