using BudgetUnderControl.Common.Contracts;
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

        Label nameLabel, accountLabel, valueLabel, tagsLabel, categoryLabel;

        public TransactionListItemDTO Transaction
        {
            get { return (TransactionListItemDTO)GetValue(TransactionProperty); }
            set { SetValue(TransactionProperty, value); }
        }

        public GroupedTransactionListItem()
        {
            var frame = new Frame { Margin = new Thickness(10)};
            var grid = new Grid { Padding = new Thickness(10), VerticalOptions = LayoutOptions.FillAndExpand };
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.4, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.4, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.2, GridUnitType.Star) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.5, GridUnitType.Star) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.5, GridUnitType.Star) });

            nameLabel = new Label { FontAttributes = FontAttributes.Bold, VerticalOptions = LayoutOptions.CenterAndExpand };
            accountLabel = new Label { VerticalOptions = LayoutOptions.CenterAndExpand, FontSize = 10 };
            valueLabel = new Label();
            tagsLabel = new Label { VerticalOptions = LayoutOptions.CenterAndExpand, FontSize = 10 };
            categoryLabel = new Label { VerticalOptions = LayoutOptions.CenterAndExpand, FontSize = 10 };

            Grid.SetRowSpan(valueLabel, 2);
            grid.Children.Add(accountLabel,0 ,1);
            grid.Children.Add(nameLabel, 0, 0);
            grid.Children.Add(categoryLabel, 1, 0);
            grid.Children.Add(tagsLabel, 1, 1);
            grid.Children.Add(valueLabel, 2, 0);
            frame.Content = grid;
            View = frame;
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
                categoryLabel.Text = Transaction.Category;
                tagsLabel.Text = string.Join(", ", Transaction.Tags.Select(x => x.Name));
            }
        }
    }
}
