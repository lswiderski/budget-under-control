using BudgetUnderControl.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace BudgetUnderControl.Mobile.Templates
{
    public class ExchangeRateListItem : ViewCell
    {
        Label fromLabel, toLabel, rateLabel, dateLabel;

        public static readonly BindableProperty ExchangeRateProperty = BindableProperty.Create(nameof(ExchangeRate), typeof(ExchangeRateDTO), typeof(ExchangeRateListItem));

        public ExchangeRateDTO ExchangeRate
        {
            get { return (ExchangeRateDTO)GetValue(ExchangeRateProperty); }
            set { SetValue(ExchangeRateProperty, value); }
        }

        public ExchangeRateListItem()
        {
            var grid = new Grid { Padding = new Thickness(10) };
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.2, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.2, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.3, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.3, GridUnitType.Star) });

            rateLabel = new Label { FontAttributes = FontAttributes.Bold };
            fromLabel = new Label();
            toLabel = new Label();
            dateLabel = new Label();

            grid.Children.Add(fromLabel);
            grid.Children.Add(toLabel, 1, 0);
            grid.Children.Add(rateLabel, 2, 0);
            grid.Children.Add(dateLabel, 3, 0);

            View = grid;
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if (BindingContext != null)
            {
                fromLabel.Text = ExchangeRate.FromCurrencyCode;
                toLabel.Text = ExchangeRate.ToCurrencyCode;
                rateLabel.Text = ExchangeRate.Rate.ToString();
                dateLabel.Text = ExchangeRate.Date.ToString("d");
            }
        }
    }
}
