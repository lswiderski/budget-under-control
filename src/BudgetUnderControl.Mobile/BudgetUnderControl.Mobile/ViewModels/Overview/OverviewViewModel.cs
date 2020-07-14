using BudgetUnderControl.CommonInfrastructure.Commands;
using BudgetUnderControl.Mobile.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BudgetUnderControl.CommonInfrastructure;
using BudgetUnderControl.CommonInfrastructure.Settings;

namespace BudgetUnderControl.ViewModel
{
    public class OverviewViewModel : IOverviewViewModel
    {
        IAccountService accountService;
        ICurrencyService currencyService;
        private readonly GeneralSettings settings;
        public OverviewViewModel(IAccountService accountModel, ICurrencyService currencyService, GeneralSettings settings)
        {
            this.accountService = accountModel;
            this.currencyService = currencyService;
            this.settings = settings;
            this.AdUnitId = settings.AdMobAdId;
        }

        public string AdUnitId { get; set; }

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

        public async Task<decimal> CalculateValueAsync(decimal amount, string fromCurrencyCode, string toCurrencyCode)
        {
            var value = await this.currencyService.TransformAmountAsync(amount, fromCurrencyCode, toCurrencyCode);
            return value;
        }
    }
}
