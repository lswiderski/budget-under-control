using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections.Generic;

namespace BudgetUnderControl.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Navigation : ContentPage
    {
        public ListView ListView { get { return listView; } }

        public Navigation()
        {
            InitializeComponent();

            var masterPageItems = new List<MasterPageItem>();
            masterPageItems.Add(new MasterPageItem
            {
                Title = "Overview",
                IconSource = "Overview.png",
                TargetType = typeof(OverviewPage)
            });
            masterPageItems.Add(new MasterPageItem
            {
                Title = "Accounts",
                IconSource = "Overview.png",
                TargetType = typeof(Accounts)
            });
            masterPageItems.Add(new MasterPageItem
            {
                Title = "Transactions",
                IconSource = "Overview.png",
                TargetType = typeof(Transactions)
            });
            masterPageItems.Add(new MasterPageItem
            {
                Title = "Settings",
                IconSource = "Settings.png",
                TargetType = typeof(SettingsPage)
            });
            masterPageItems.Add(new MasterPageItem
            {
                Title = "Currencies",
                IconSource = "Settings.png",
                TargetType = typeof(Currencies)
            });
            
            listView.ItemsSource = masterPageItems;
        }
    }
}