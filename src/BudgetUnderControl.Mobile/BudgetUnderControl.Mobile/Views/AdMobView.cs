using Autofac;
using BudgetUnderControl.CommonInfrastructure.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace BudgetUnderControl.Views
{
	public class AdMobView : View
	{
        public AdMobView()
        {
			using (var scope = App.Container.BeginLifetimeScope())
			{
				var settings = scope.Resolve<GeneralSettings>();

				this.AdUnitId = settings.AdMobAdId;
			}

		}
		public static readonly BindableProperty AdUnitIdProperty = BindableProperty.Create(
					   nameof(AdUnitId),
					   typeof(string),
					   typeof(AdMobView),
					   string.Empty);

		public string AdUnitId { get; set; }
	}
}