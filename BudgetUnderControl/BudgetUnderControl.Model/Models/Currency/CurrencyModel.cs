using BudgetUnderControl.Domain;
using BudgetUnderControl.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetUnderControl.Model
{
    
    public class CurrencyModel : BaseModel, ICurrencyModel
    {
        IContextConfig contextConfig;
        public CurrencyModel(IContextConfig contextConfig) : base(contextConfig)
        {
            this.contextConfig = contextConfig;
        }

        public async Task<ICollection<CurrencyDTO>> GetCurriences()
        {
            var list = this.Context.Currencies.Select(x => new CurrencyDTO
            {
                Code = x.Code,
                Id = x.Id,
                Name = x.FullName,
            }).ToListAsync();

            return await list;
        }
    }
}
