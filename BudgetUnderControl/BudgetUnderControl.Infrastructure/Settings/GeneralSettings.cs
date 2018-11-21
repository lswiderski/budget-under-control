using BudgetUnderControl.Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetUnderControl.Infrastructure.Settings
{
    public class GeneralSettings
    {
        public string AppName { get; set; }
        public string DbName { get; set; }
        public ApplicationType ApplicationType { get; set; }
        public string ApplicationId { get; set; }
        public string ConnectionString { get; set; }
    }
}
