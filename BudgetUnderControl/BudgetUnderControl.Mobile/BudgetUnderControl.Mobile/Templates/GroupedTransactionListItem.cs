using BudgetUnderControl.Common.Contracts;
using BudgetUnderControl.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BudgetUnderControl.Views
{
    class GroupedTransactionListItem : ViewCell
    {

        public static readonly BindableProperty TransactionProperty = BindableProperty.Create("GroupedTransaction", typeof(TransactionListItemDTO), typeof(GroupedTransactionListItem));

        Label nameLabel, accountLabel, valueLabel;

        public TransactionListItemDTO Transaction
        {
            get { return (TransactionListItemDTO)GetValue(TransactionProperty); }
            set { SetValue(TransactionProperty, value); }
        }

        public GroupedTransactionListItem()
        {
            var grid = new Grid { Padding = new Thickness(10), VerticalOptions = LayoutOptions.FillAndExpand };
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.4, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.4, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.2, GridUnitType.Star) });


            nameLabel = new Label { FontAttributes = FontAttributes.Bold, VerticalOptions = LayoutOptions.CenterAndExpand };
            accountLabel = new Label { VerticalOptions = LayoutOptions.CenterAndExpand, FontSize = 10 };
            valueLabel = new Label();

            grid.Children.Add(accountLabel,0 ,0);
            grid.Children.Add(nameLabel, 1, 0);
            grid.Children.Add(valueLabel, 2, 0);

            View = grid;
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if (BindingContext != null)
            {
                var value = Transaction.ValueWithCurrency;
                if (Transaction.Value < 0)
                {
                    value = value.Remove(0, 1);
                }

                nameLabel.Text = Transaction.Name;
                accountLabel.Text = Transaction.Account;
                valueLabel.Text = value;
                valueLabel.TextColor = Transaction.Type == Common.Enums.TransactionType.Income ? Color.Green : Color.Red;
            }
        }
    }
}
