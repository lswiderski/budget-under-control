using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BudgetUnderControl.Views
{
    class TransactionListItem : ViewCell
    {
        public static readonly BindableProperty NameProperty = BindableProperty.Create("Name", typeof(string), typeof(TransactionListItem), "Name");
        public static readonly BindableProperty AccountProperty = BindableProperty.Create("Account", typeof(string), typeof(TransactionListItem), "Account");
        public static readonly BindableProperty ValueProperty = BindableProperty.Create("Value", typeof(string), typeof(TransactionListItem), "Value");

        Label nameLabel, accountLabel, valueLabel;

        public string Name
        {
            get { return (string)GetValue(NameProperty); }
            set { SetValue(NameProperty, value); }
        }

        public string Account
        {
            get { return (string)GetValue(AccountProperty); }
            set { SetValue(AccountProperty, value); }
        }

        public string Amount
        {
            get { return (string)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public TransactionListItem()
        {
            var grid = new Grid { Padding = new Thickness(10) };
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.5, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.2, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.3, GridUnitType.Star) });

            nameLabel = new Label { FontAttributes = FontAttributes.Bold };
            accountLabel = new Label();
            valueLabel = new Label();

            grid.Children.Add(nameLabel);
            grid.Children.Add(accountLabel, 1, 0);
            grid.Children.Add(valueLabel, 2, 0);
            View = grid;
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if (BindingContext != null)
            {
                nameLabel.Text = Name;
                accountLabel.Text = Account;
                valueLabel.Text = Amount;
            }
        }
    }
}
