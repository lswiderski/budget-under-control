using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BudgetUnderControl.Views
{
    public class AccountListItem : ViewCell
    {
        public static readonly BindableProperty NameProperty = BindableProperty.Create("Name", typeof(string), typeof(CurrencyListItem), "Name");
        public static readonly BindableProperty CurrencyProperty = BindableProperty.Create("Currency", typeof(string), typeof(CurrencyListItem), "Currency");
        public static readonly BindableProperty BalanceProperty = BindableProperty.Create("Balance", typeof(string), typeof(CurrencyListItem), "Balance");

        Label nameLabel, currencyLabel, balance;

        public string Name
        {
            get { return (string)GetValue(NameProperty); }
            set { SetValue(NameProperty, value); }
        }

        public string Currency
        {
            get { return (string)GetValue(CurrencyProperty); }
            set { SetValue(CurrencyProperty, value); }
        }

        public string Amount
        {
            get { return (string)GetValue(BalanceProperty); }
            set { SetValue(BalanceProperty, value); }
        }

        public AccountListItem()
        {
            var grid = new Grid { Padding = new Thickness(10) };
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.5, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.2, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.3, GridUnitType.Star) });

            nameLabel = new Label { FontAttributes = FontAttributes.Bold };
            currencyLabel = new Label();
            balance = new Label();

            grid.Children.Add(nameLabel);
            grid.Children.Add(currencyLabel, 1, 0);
            grid.Children.Add(balance, 2, 0);
            View = grid;
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if (BindingContext != null)
            {
                nameLabel.Text = Name;
                currencyLabel.Text = Currency;
                balance.Text = Amount;
            }
        }
    }
}
