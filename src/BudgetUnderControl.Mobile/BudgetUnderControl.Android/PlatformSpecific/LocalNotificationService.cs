using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using BudgetUnderControl.Common.Contracts.System;
using BudgetUnderControl.Droid.PlatformSpecific;
using BudgetUnderControl.Mobile.PlatformSpecific;
using BudgetUnderControl.Views;
using Java.Lang;
using Xamarin.Forms;

[assembly: Dependency(typeof(LocalNotificationService))]
namespace BudgetUnderControl.Droid.PlatformSpecific
{
    public class LocalNotificationService : ILocalNotificationService
    {
        string _packageName => Android.App.Application.Context.PackageName;
        NotificationManager _manager => (NotificationManager)Android.App.Application.Context.GetSystemService(Context.NotificationService);

        /// <summary>
        /// Get or Set Resource Icon to display
        /// </summary>
        public static int NotificationIconId { get; set; }
        public static string ChannelName = "General";
        public static string ChannelDescription = "General";
        public static string CHANNEL_ID = "BUC_General";
        private Random random = new Random();


        public void ShowNotification(string title, string body, IEnumerable<BundleItem> bundleItems = null)
        {
            this.CreateNotificationChannel();

            var notificationId = random.Next();

            var builder = new Notification.Builder(Android.App.Application.Context, CHANNEL_ID);
            builder.SetContentTitle(title);
            builder.SetContentText(body);
            builder.SetAutoCancel(true);

            if (NotificationIconId != 0)
            {
                builder.SetSmallIcon(NotificationIconId);
            }
            else
            {
                builder.SetSmallIcon(Resource.Drawable.plugin_lc_smallicon);
            }

            var valuesForActivity = this.GetBundle(bundleItems);

            //var resultIntent = GetLauncherActivity();
            var resultIntent = new Intent(Android.App.Application.Context, Class.FromType(typeof(MainActivity)));
            //resultIntent.SetFlags(ActivityFlags.NewTask | ActivityFlags.ClearTask);
         
            var stackBuilder = Android.Support.V4.App.TaskStackBuilder.Create(Android.App.Application.Context);
            stackBuilder.AddParentStack(Class.FromType(typeof(MainActivity)));
            stackBuilder.AddNextIntent(resultIntent);
            resultIntent.PutExtras(valuesForActivity);
            var resultPendingIntent = stackBuilder.GetPendingIntent(0, (int)PendingIntentFlags.UpdateCurrent);
            builder.SetContentIntent(resultPendingIntent);

            _manager.Notify(notificationId, builder.Build());
        }

        public static Intent GetLauncherActivity()
        {
            var packageName = Android.App.Application.Context.PackageName;
            return Android.App.Application.Context.PackageManager.GetLaunchIntentForPackage(packageName);
        }

        private Bundle GetBundle(IEnumerable<BundleItem> items)
        {
            var bundle = new Bundle();

            if(items != null)
            {
                foreach (var item in items)
                {
                    switch (item.Type)
                    {
                        case Common.Enums.BundleItemType.Bool:
                            bundle.PutBoolean(item.Key, (bool)item.Object);
                            break;
                        case Common.Enums.BundleItemType.Int:
                            bundle.PutInt(item.Key, (int)item.Object);
                            break;
                        case Common.Enums.BundleItemType.Long:
                            bundle.PutLong(item.Key, (long)item.Object);
                            break;
                        case Common.Enums.BundleItemType.String:
                            bundle.PutString(item.Key, (string)item.Object);
                            break;
                        default:
                            break;
                    }
                }
            }
           

            return bundle;
        }

        private void CreateNotificationChannel()
        {
            if (Build.VERSION.SdkInt < BuildVersionCodes.O)
            {
                // Notification channels are new in API 26 (and not a part of the
                // support library). There is no need to create a notification
                // channel on older versions of Android.
                return;
            }

            var name = ChannelName;
            var description = ChannelDescription;
            var channel = new NotificationChannel(CHANNEL_ID, name, NotificationImportance.Default)
            {
                Description = description
            };
            _manager.CreateNotificationChannel(channel);
        }

    }
}