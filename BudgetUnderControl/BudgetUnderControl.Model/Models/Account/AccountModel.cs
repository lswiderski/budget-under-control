using BudgetUnderControl.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetUnderControl.Model
{
    public class AccountModel : BaseModel, IAccountModel
    {

        IContextConfig contextConfig;
        public AccountModel(IContextConfig contextConfig) : base(contextConfig)
        {
            this.contextConfig = contextConfig;
        }

        
            public async Task<ICollection<AccountListItemViewModel>> GetAccounts()
        {
            var list = (from account in this.Context.Accounts
                        join currency in this.Context.Currencies on account.CurrencyId equals currency.Id
                        select new AccountListItemViewModel
                        {
                             Currency = currency.Code,
                             CurrencyId = currency.Id,
                             CurrencySymbol = currency.Symbol,
                             Id = account.Id,
                             IsIncludedInTotal = account.IsIncludedToTotal,
                             Name = account.Name,
                             Amount = this.Context.Transactions.Where(x => x.AccountId == account.Id).Sum(x => x.Amount),
                        }
                        ).ToListAsync();

            return await list;
        }
    }
}
