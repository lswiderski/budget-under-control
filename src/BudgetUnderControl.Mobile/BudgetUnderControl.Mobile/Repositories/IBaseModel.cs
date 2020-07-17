using BudgetUnderControl.MobileDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetUnderControl.Mobile
{
    public interface IBaseModel
    {
        IContextFacade Context { get;}
    }
}
