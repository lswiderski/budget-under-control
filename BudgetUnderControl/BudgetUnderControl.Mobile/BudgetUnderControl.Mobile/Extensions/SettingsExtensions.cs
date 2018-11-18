using Autofac;
using BudgetUnderControl.Common;
using BudgetUnderControl.Mobile.IoC;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace BudgetUnderControl.Mobile.Extensions
{
    public static class SettingsExtensions
    {
        public static T GetSettings<T>(this MobileModule module, string fileName) where T : new()
        {
            IFileHelper fileHelper = DependencyService.Get<IFileHelper>();
            var configurationFile = fileHelper.ReadFromAssetsAsString(fileName);
            var configurationObject = JsonConvert.DeserializeObject<T>(configurationFile);
            return configurationObject;


        }
    }
}
