using BudgetUnderControl.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetUnderControl.ViewModel
{
    public class OverviewViewModel : IOverviewViewModel
    {
        IAccountModel accountModel;
        public OverviewViewModel(IAccountModel accountModel)
        {
            this.accountModel = accountModel;
        }

        public  Dictionary<string, decimal> GetTotals()
        {
            Dictionary<string, decimal> result = new Dictionary<string, decimal>();
            var accounts = accountModel.GetAccounts();

            foreach (var account in accounts)
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

            return result;
        }
    }
}
