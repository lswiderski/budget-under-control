using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BudgetUnderControl.Common.Contracts;
using BudgetUnderControl.Infrastructure.Commands;
using BudgetUnderControl.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BudgetUnderControl.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ApiControllerBase
    {
        private IAccountService accountService;

        public AccountsController(IAccountService accountService, ICommandDispatcher commandDispatcher) : base(commandDispatcher)
        {
            this.accountService = accountService;
        }

        // GET api/accounts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AccountListItemDTO>>> Get()
        {
            var accounts = await this.accountService.GetAccountsWithBalanceAsync();
            return Ok(accounts.ToList());
        }

        // GET api/accounts/552cbd7c-e9d9-46c9-ab7e-2b10ae38ab4a
        [HttpGet("{id}")]
        public async Task<ActionResult<AccountDetailsDTO>> GetAccount(Guid id)
        {
            var account = await this.accountService.GetAccountAsync(id);
            return Ok(account);
        }


        // GET api/accounts/552cbd7c-e9d9-46c9-ab7e-2b10ae38ab4a/Details
        [HttpGet("{id}/details")]
        public async Task<ActionResult<AccountDetailsDTO>> GetAccountDetails(Guid id)
        {
            var account = await this.accountService.GetAccountDetailsAsync(new TransactionsFilter { AccountsExternalIds = new HashSet<Guid> { id } });
            return Ok(account);
        }

        // POST api/accounts
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] AddAccount command)
        {
            await this.DispatchAsync(command);

            return CreatedAtAction(nameof(Get), new { id = command.ExternalId }, command);
        }

        // PUT api/accounts/552cbd7c-e9d9-46c9-ab7e-2b10ae38ab4a
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] EditAccount command)
        {
            await this.DispatchAsync(command);
            return NoContent();
        }

        // DELETE api/accounts/552cbd7c-e9d9-46c9-ab7e-2b10ae38ab4a
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await this.DispatchAsync(new DeleteAccount { Id = id });
            return NoContent();
        }
    }

}