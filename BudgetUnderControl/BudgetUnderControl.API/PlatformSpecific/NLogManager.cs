using BudgetUnderControl.Common;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BudgetUnderControl.API
{
    public class NLogManager : ILogManager
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        public NLogManager()
        {

        }

        public Common.ILogger GetLog(string callerFilePath = "")
        {
            return new NLogLogger(Logger);
        }

       
    }
}
