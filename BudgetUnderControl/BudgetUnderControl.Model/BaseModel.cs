using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BudgetUnderControl.Domain;
using BudgetUnderControl.Common;
using Microsoft.EntityFrameworkCore;

namespace BudgetUnderControl.Model
{
    public class BaseModel : IBaseModel
    {
        private Context _context;
        private IContextConfig contextConfig;
        public Context Context
        {
            get
            {
                return _context;
            }
        }
        public BaseModel(IContextConfig contextConfig)
        {
            _context = new Context(contextConfig);
            //Context.Database.EnsureCreated();
            Context.Database.Migrate();
        }
    }
}
