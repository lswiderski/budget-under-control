using BudgetUnderControl.Model.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BudgetUnderControl.Infrastructure.Commands
{
    class EditTransactionHandler : ICommandHandler<EditTransaction>
    {
        private readonly ITransactionService transactionService;
        public EditTransactionHandler(ITransactionService transactionService)
        {
            this.transactionService = transactionService;
        }

        public async Task HandleAsync(EditTransaction command)
        {
            await this.transactionService.EditTransactionAsync(command);

        }
    }
}