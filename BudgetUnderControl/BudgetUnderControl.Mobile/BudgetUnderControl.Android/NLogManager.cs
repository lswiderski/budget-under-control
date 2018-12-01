
using BudgetUnderControl.Droid;
using BudgetUnderControl.Mobile;
using NLog;
using NLog.Config;
using NLog.Targets;
using System;
using System.IO;
using Xamarin.Forms;

[assembly: Dependency(typeof(NLogManager))]
namespace BudgetUnderControl.Droid
{
    public class NLogManager : ILogManager
    {
        public NLogManager()
        {
            var config = new LoggingConfiguration();

            var consoleTarget = new ConsoleTarget();
            config.AddTarget("console", consoleTarget);

            var consoleRule = new LoggingRule("*", LogLevel.Trace, consoleTarget);
            config.LoggingRules.Add(consoleRule);

            var fileTarget = new FileTarget();

            string folder = Android.OS.Environment.ExternalStorageDirectory.Path;
            fileTarget.FileName = Path.Combine(folder, "BudgetUnderControl", "buc-Log.txt");
            config.AddTarget("file", fileTarget);

            var fileRule = new LoggingRule("*", LogLevel.Warn, fileTarget);
            config.LoggingRules.Add(fileRule);

            LogManager.Configuration = config;
        }

        public Mobile.ILogger GetLog([System.Runtime.CompilerServices.CallerFilePath] string callerFilePath = "")
        {
            string fileName = callerFilePath;

            if (fileName.Contains("/"))
            {
                fileName = fileName.Substring(fileName.LastIndexOf("/", StringComparison.CurrentCultureIgnoreCase) + 1);
            }

            var logger = LogManager.GetLogger(fileName);
            return new NLogLogger(logger);
        }
    }
}