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
        private IContextFacade _context;
        private IContextConfig contextConfig;
        public IContextFacade Context
        {
            get
            {
                return _context;
            }
        }
        public BaseModel(IContextFacade context)
        {
            _context = context;
        }
    }
}
