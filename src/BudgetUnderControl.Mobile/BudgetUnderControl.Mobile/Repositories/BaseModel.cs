using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BudgetUnderControl.MobileDomain;
using BudgetUnderControl.Common;
using Microsoft.EntityFrameworkCore;

namespace BudgetUnderControl.Mobile
{
    public class BaseModel : IBaseModel
    {
        public IContextFacade Context { get; }

        public BaseModel(IContextFacade context)
        {
            Context = context;
        }
    }
}
