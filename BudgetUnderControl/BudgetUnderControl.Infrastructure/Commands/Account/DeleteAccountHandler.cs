using BudgetUnderControl.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BudgetUnderControl.Infrastructure.Commands
{
    public class DeleteAccountHandler : ICommandHandler<DeleteAccount>
    {
        private readonly IAccountService accountService;
        public DeleteAccountHandler(IAccountService accountService)
        {
            this.accountService = accountService;
        }

        public async Task HandleAsync(DeleteAccount command)
        {
            await this.accountService.DeleteAccountAsync(command);

        }
    }
}
