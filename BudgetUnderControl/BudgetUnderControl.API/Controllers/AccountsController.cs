using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BudgetUnderControl.Common.Contracts;
using BudgetUnderControl.Infrastructure.Commands;
using BudgetUnderControl.Model.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BudgetUnderControl.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private IAccountService accountService;

        public AccountsController(IAccountService accountService)
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

        // GET api/accounts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AccountDetailsDTO>> GetAccountDetails(int id)
        {
            var account = await this.accountService.GetAccountDetailsAsync(new TransactionsFilter {AccountsIds = new HashSet<int> { id }});
            return Ok(account);
        }

    }
}