using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BudgetUnderControl.Infrastructure.Commands;
using BudgetUnderControl.Infrastructure.Services;
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

        public LoginController(ICommandDispatcher commandDispatcher, IMemoryCache cache) : base(commandDispatcher)
        {
            this.cache = cache;
        }

        [HttpPost("Mobile")]
        public async Task<IActionResult> Post([FromBody]MobileLoginCommand command)
        {
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