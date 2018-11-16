using BudgetUnderControl.Common.Contracts;
using BudgetUnderControl.Infrastructure.Commands;
using BudgetUnderControl.Model.Services;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BudgetUnderControl.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ApiControllerBase
    {
        private readonly ITransactionService transactionService;

        public TransactionsController(ITransactionService transactionService, ICurrencyService currencyService, ICommandDispatcher commandDispatcher) : base(commandDispatcher)
        {
            this.transactionService = transactionService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TransactionListItemDTO>>> Get()
        {
            var transactions = await this.transactionService.GetTransactionsAsync();
            return transactions.ToList();
        }

        // POST api/transactions
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] AddTransaction command)
        {
            await this.DispatchAsync(command);

            //return Created($"transactions/{command.ExternalId}", command);
            return CreatedAtAction(nameof(Get), new { id = command.ExternalId }, command);
        }

        // GET api/transactions/552cbd7c-e9d9-46c9-ab7e-2b10ae38ab4a
        [HttpGet("{id}")]
        public async Task<ActionResult<EditTransactionDTO>> GetById(Guid id)
        {
            var transaction = await this.transactionService.GetTransactionAsync(id);
            return Ok(transaction);
        }

        // PUT api/transactions/552cbd7c-e9d9-46c9-ab7e-2b10ae38ab4a
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] EditTransaction command)
        {
            await this.DispatchAsync(command);
            return NoContent();
        }

        // DELETE api/transactions/552cbd7c-e9d9-46c9-ab7e-2b10ae38ab4a
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await this.DispatchAsync(new DeleteTransaction { ExternalId = id });
            return NoContent();
        }
    }
}
