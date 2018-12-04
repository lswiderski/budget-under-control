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
        private static Logger Logger = LogManager.GetCurrentClassLogger();
        public NLogManager()
        {

        }
        /// <summary>
        /// Returning Nlog logger. Param useless at Web implementation
        /// </summary>
        /// <param name="callerFilePath">leave empty</param>
        /// <returns></returns>
        public ILogger GetLog(string callerFilePath = "")
        {

            return Logger;
        }

       
    }
}
