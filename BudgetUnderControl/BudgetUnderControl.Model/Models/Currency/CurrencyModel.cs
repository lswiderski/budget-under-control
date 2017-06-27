using BudgetUnderControl.Domain;
using BudgetUnderControl.Model;
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

        public ICollection<CurrencyViewModel> GetCurriences()
        {
            var list = this.Context.Currencies.Select(x => new CurrencyViewModel
            {
                Code = x.Code,
                Id = x.Id,
                Name = x.FullName,
            }).ToList();

            return list;
        }
    }
}
