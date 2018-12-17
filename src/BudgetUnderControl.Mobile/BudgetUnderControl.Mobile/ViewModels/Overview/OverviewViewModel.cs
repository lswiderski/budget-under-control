using BudgetUnderControl.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetUnderControl.ViewModel
{
    public class OverviewViewModel : IOverviewViewModel
    {
        IAccountService accountService;
        public OverviewViewModel(IAccountService accountModel)
        {
            this.accountService = accountModel;
        }

        public async Task<Dictionary<string, decimal>> GetTotalsAsync()
        {
            Dictionary<string, decimal> result = new Dictionary<string, decimal>();
            var accounts = await accountService.GetAccountsWithBalanceAsync();

            foreach (var account in accounts)
            {
                if(!account.ParentAccountId.HasValue)
                {
                    if (!result.ContainsKey(account.Currency))
                    {
                        result.Add(account.Currency, account.Balance);
                    }
                    else
                    {
                        result[account.Currency] += account.Balance;
                    }
                }

            }

            return result;
        }
    }
}
