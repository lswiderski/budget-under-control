using BudgetUnderControl.Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetUnderControl.Common.Contracts.System
{
    public class BundleItem
    {
        public object Object { get; set; }
        public string Key { get; set; }
        public BundleItemType Type { get; set; }
    }
}
