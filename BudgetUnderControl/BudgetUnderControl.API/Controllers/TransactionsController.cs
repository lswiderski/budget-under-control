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
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionService transactionService;

        public TransactionsController(ITransactionService transactionService, ICurrencyService currencyService)
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
        public IActionResult Post([FromBody] AddTransactionCommand value)
        {

            return Ok();
        }

    }
}
