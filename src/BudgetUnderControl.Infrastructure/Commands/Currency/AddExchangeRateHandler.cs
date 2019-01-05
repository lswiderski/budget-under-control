using BudgetUnderControl.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BudgetUnderControl.Infrastructure.Commands
{
    public class AddExchangeRateHandler : ICommandHandler<AddExchangeRate>
    {
        private readonly ICurrencyService currencyService;
        public AddExchangeRateHandler(ICurrencyService currencyService)
        {
            this.currencyService = currencyService;
        }

        public async Task HandleAsync(AddExchangeRate command)
        {
            await this.currencyService.AddExchangeRateAsync(command);
        }
    }
}
