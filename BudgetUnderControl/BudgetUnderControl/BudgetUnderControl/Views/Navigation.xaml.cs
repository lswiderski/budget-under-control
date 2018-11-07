using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

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
                IconSource = "Accounts.png",
                TargetType = typeof(Accounts)
            });
            masterPageItems.Add(new MasterPageItem
            {
                Title = "Transactions",
                IconSource = "Transactions.png",
                TargetType = typeof(Transactions)
            });
            
            masterPageItems.Add(new MasterPageItem
            {
                Title = "Currencies",
                IconSource = "Currencies.png",
                TargetType = typeof(Currencies)
            });
            masterPageItems.Add(new MasterPageItem
            {
                Title = "Settings",
                IconSource = "Settings.png",
                TargetType = typeof(SettingsPage)
            });

            listView.ItemsSource = masterPageItems;
        }
    }
}