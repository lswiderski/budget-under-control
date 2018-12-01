using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetUnderControl.Mobile
{
    public interface ILogManager
    {
        ILogger GetLog([System.Runtime.CompilerServices.CallerFilePath]string callerFilePath = "");
    }
}
