using BudgetUnderControl.Common.Contracts;
using BudgetUnderControl.Domain;
using BudgetUnderControl.Domain.Repositiories;
using BudgetUnderControl.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetUnderControl.Infrastructure
{
    
    public class CurrencyRepository : BaseModel, ICurrencyRepository
    {
        public CurrencyRepository(IContextFacade context) : base(context)
        {
        }

        public async Task AddCurrencyAsync(Currency currency)
        {
            this.Context.Currencies.Add(currency);
            await this.Context.SaveChangesAsync();
        }

        public async Task<ICollection<Currency>> GetCurriencesAsync()
        {
            var list = await this.Context.Currencies.ToListAsync();

            return list;
        }

        public async Task<Currency> GetCurrencyAsync(int id)
        {
            var currency = await this.Context.Currencies.FirstOrDefaultAsync(x => x.Id == id);

            return currency;
        }
    }
}
