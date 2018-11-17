using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BudgetUnderControl.Infrastructure.Commands;
using BudgetUnderControl.Infrastructure;
using BudgetUnderControl.Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BudgetUnderControl.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsGroupsController : ApiControllerBase
    {

        private IAccountGroupService accountGroupService;

        public AccountsGroupsController(IAccountGroupService accountGroupService, ICommandDispatcher commandDispatcher) : base(commandDispatcher)
        {
            this.accountGroupService = accountGroupService;
        }

        // GET api/accountsgroups
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AccountGroupItemDTO>>> Get()
        {
            var accountsGroups = await this.accountGroupService.GetAccountGroupsAsync();
            return Ok(accountsGroups.ToList());
        }

        // GET api/accounts/552cbd7c-e9d9-46c9-ab7e-2b10ae38ab4a
        [HttpGet("{id}")]
        public async Task<ActionResult<AccountGroupItemDTO>> GetAccount(Guid id)
        {
            var account = await this.accountGroupService.GetAccountGroupAsync(id);
            return Ok(account);
        }
        
    }
}