using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BudgetUnderControl.Infrastructure.Commands;
using BudgetUnderControl.Infrastructure.Services;
using BudgetUnderControl.Infrastructure.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace BudgetUnderControl.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ApiControllerBase
    {
        private readonly IMemoryCache cache;
        private readonly GeneralSettings settings;

        public LoginController(ICommandDispatcher commandDispatcher, GeneralSettings settings, IMemoryCache cache) : base(commandDispatcher)
        {
            this.cache = cache;
            this.settings = settings;
        }

        [HttpPost("Mobile")]
        public async Task<IActionResult> Post([FromBody]MobileLoginCommand command)
        {
            if (Request.Headers["Api-Key"] != settings.ApiKey)
            {
                return Unauthorized();
            }

            command.TokenId = Guid.NewGuid();
            await DispatchAsync(command);
            var userId = cache.Get<Guid>(command.TokenId);

            if(userId == null || userId == Guid.Empty)
            {
                return Unauthorized();
            }

            return Ok(userId);
        }
    }
}