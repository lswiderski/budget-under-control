using BudgetUnderControl.Common.Contracts;
using BudgetUnderControl.Domain;
using BudgetUnderControl.Domain.Repositiories;
using BudgetUnderControl.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetUnderControl.Model
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
    }
}
