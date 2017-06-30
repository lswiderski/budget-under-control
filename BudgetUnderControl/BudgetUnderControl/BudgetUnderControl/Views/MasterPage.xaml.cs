using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using BudgetUnderControl;

namespace BudgetUnderControl.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MasterPage : MasterDetailPage
    {
        public MasterPage()
        {
            InitializeComponent();

            navigation.ListView.ItemSelected += OnItemSelected;
        }

        void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as MasterPageItem;
            if (item != null)
            {
                NavigateTo(item.TargetType);
                navigation.ListView.SelectedItem = null;
                IsPresented = false;
            }
        }

        public void NavigateTo(Type targetType, params object[] args)
        {
            Detail = new NavigationPage((Page)Activator.CreateInstance(targetType, args));
        }

        public void NavigateTo(Type targetType)
        {
            Detail = new NavigationPage((Page)Activator.CreateInstance(targetType));
        }
    }
}