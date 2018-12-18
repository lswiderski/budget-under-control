using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BudgetUnderControl.Common.Contracts;
using BudgetUnderControl.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BudgetUnderControl.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CurrenciesController : ControllerBase
    {
        private readonly ICurrencyService currencyService;

        public CurrenciesController(ICurrencyService currencyService)
        {
            this.currencyService = currencyService;
        }

        // GET api/currencies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CurrencyDTO>>> Get()
        {
            var currencies = await this.currencyService.GetCurriencesAsync();
            return currencies.ToList();
        }

        // GET api/currencies/1
        [HttpGet("{id}")]
        public async Task<ActionResult<EditTransactionDTO>> GetById(int id)
        {
            var category = await this.currencyService.GetCurrencyAsync(id);
            return Ok(category);
        }

    }
}