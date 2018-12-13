using BudgetUnderControl.Common.Contracts.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetUnderControl.Mobile.PlatformSpecific
{
    public interface ILocalNotificationService
    {
        void ShowNotification(string title, string body, IEnumerable<BundleItem> bundleItems = null);
    }
}
