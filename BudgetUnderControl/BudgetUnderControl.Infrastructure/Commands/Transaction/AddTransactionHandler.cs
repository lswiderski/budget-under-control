using BudgetUnderControl.Model.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BudgetUnderControl.Infrastructure.Commands
{
    public class AddTransactionHandler : ICommandHandler<AddTransaction>
    {
        private readonly ITransactionService transactionService;
        public AddTransactionHandler(ITransactionService transactionService)
        {
            this.transactionService = transactionService;
        }

        public async Task HandleAsync(AddTransaction command)
        {
            await this.transactionService.AddTransactionAsync(command);
            
        }
    }
}
