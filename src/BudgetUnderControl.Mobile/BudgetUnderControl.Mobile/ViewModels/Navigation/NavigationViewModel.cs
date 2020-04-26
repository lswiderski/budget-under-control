using BudgetUnderControl.Mobile.Keys;
using BudgetUnderControl.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace BudgetUnderControl.Mobile
{
    public class NavigationViewModel : INavigationViewModel, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public bool IsLogged
        {
            get => Preferences.Get(PreferencesKeys.IsUserLogged, false);
            set
            {
                if (Preferences.Get(PreferencesKeys.IsUserLogged, false) != value)
                {
                    Preferences.Set(PreferencesKeys.IsUserLogged, value);
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsLogged)));
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsNotLogged)));
                }
            }
        }

        public bool IsNotLogged
        {
            get => !IsLogged;
            set
            {
                if (IsLogged != value)
                {
                    IsLogged = !value;
                }
            }
        }

        public List<MasterPageItem> MasterPageItems {get;set;}

        public NavigationViewModel()
        {

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
                Title = "Tags",
                IconSource = "Tags.png",
                TargetType = typeof(Tags)
            });
            masterPageItems.Add(new MasterPageItem
            {
                Title = "Settings",
                IconSource = "Settings.png",
                TargetType = typeof(SettingsPage)
            });
            MasterPageItems = masterPageItems;
           
            //listView.ItemsSource = masterPageItems;
        }

        public void RefreshUserButtons()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsLogged)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsNotLogged)));
        }
    }
}
