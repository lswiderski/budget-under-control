using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Service.Notification;
using Android.Views;
using Android.Widget;
using BudgetUnderControl.Common;
using NLog;
using Xamarin.Forms;

namespace BudgetUnderControl.Droid.PlatformSpecific
{
    [Service(Label = "Budget Under Control Notification Listener", Permission ="android.permission.BIND_NOTIFICATION_LISTENER_SERVICE")]
    [IntentFilter(new[] { "android.service.notification.NotificationListenerService" })]
    class NLService : NotificationListenerService
    {
        private static ILogger logger;

        public override void OnCreate()
        {
            base.OnCreate();
            logger = DependencyService.Get<ILogManager>().GetLog();
        }

        public override void OnNotificationPosted(StatusBarNotification sbn)
        {
            base.OnNotificationPosted(sbn);
            
            string pack = sbn.PackageName;
            string ticker = sbn.Notification.TickerText?.ToString();
            Bundle extras = sbn.Notification.Extras;
            string title = extras.GetString("android.title");
            string text = extras.GetCharSequence("android.text")?.ToString();
            logger.Info($"pack: {pack} | ticker: {ticker} | title: {title} | text: {text}");
            Toast.MakeText(this.ApplicationContext, "Notification showed up", ToastLength.Short);

        }

        public override void OnNotificationRemoved(StatusBarNotification sbn)
        {
            base.OnNotificationRemoved(sbn);
            logger.Info(sbn.Notification.ToString());
        }
    }
}