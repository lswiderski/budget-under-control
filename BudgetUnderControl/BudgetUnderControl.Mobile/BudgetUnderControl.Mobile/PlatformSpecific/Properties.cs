using BudgetUnderControl.Common;
using BudgetUnderControl.Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace BudgetUnderControl.Mobile.PlatformSpecific
{
    public static class Properties
    {
        public static ActivityPage? REDIRECT_TO
        {
            get
            {
                if (Application.Current.Properties.ContainsKey(PropertyKeys.REDIRECT_TO))
                {
                    return (ActivityPage)Application.Current.Properties[PropertyKeys.REDIRECT_TO];
                }
                else
                {
                    return null;
                }
            }

            set
            {
                Application.Current.Properties[PropertyKeys.REDIRECT_TO] = value;

                Application.Current.SavePropertiesAsync();
            }
        }

        public static string ADD_TRANSACTION_VALUE
        {
            get
            {
                if (Application.Current.Properties.ContainsKey(PropertyKeys.ADD_TRANSACTION_VALUE))
                {
                    return (string)Application.Current.Properties[PropertyKeys.ADD_TRANSACTION_VALUE];
                }
                else
                {
                    return string.Empty;
                }
            }

            set
            {
                Application.Current.Properties[PropertyKeys.ADD_TRANSACTION_VALUE] = value;

                Application.Current.SavePropertiesAsync();
            }
        }

        public static string ADD_TRANSACTION_TITLE
        {
            get
            {
                if (Application.Current.Properties.ContainsKey(PropertyKeys.ADD_TRANSACTION_TITLE))
                {
                    return (string)Application.Current.Properties[PropertyKeys.ADD_TRANSACTION_TITLE];
                }
                else
                {
                    return string.Empty;
                }
            }

            set
            {
                Application.Current.Properties[PropertyKeys.ADD_TRANSACTION_TITLE] = value;

                Application.Current.SavePropertiesAsync();
            }
        }
    }
}
