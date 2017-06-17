using BudgetUnderControl.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetUnderControl.Model
{
    class BaseModel
    {
        private Context context;
        protected Context Context { get { return context; }  }
        public BaseModel(IContextConfig contextConfig)
        {
            context = new Context(contextConfig);
            Context.Create(contextConfig.DBPath);
        }
    }
}
