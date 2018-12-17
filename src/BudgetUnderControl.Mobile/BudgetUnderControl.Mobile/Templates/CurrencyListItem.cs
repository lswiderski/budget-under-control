using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BudgetUnderControl.Views
{
    public class CurrencyListItem : ViewCell
    {
        Label nameLabel, codeLabel;

        public static readonly BindableProperty NameProperty = BindableProperty.Create("Name", typeof(string), typeof(CurrencyListItem), "Name");
        public static readonly BindableProperty CodeProperty = BindableProperty.Create("Code", typeof(string), typeof(CurrencyListItem), "");

        public string Name
        {
            get { return (string)GetValue(NameProperty); }
            set { SetValue(NameProperty, value); }
        }

        public string Code
        {
            get { return (string)GetValue(CodeProperty); }
            set { SetValue(CodeProperty, value); }
        }



        public CurrencyListItem()
        {
            var grid = new Grid { Padding = new Thickness(10) };
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.5, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.2, GridUnitType.Star) });

            nameLabel = new Label { FontAttributes = FontAttributes.Bold };
            codeLabel = new Label();

            grid.Children.Add(nameLabel);
            grid.Children.Add(codeLabel, 1, 0);

            View = grid;
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if (BindingContext != null)
            {
                nameLabel.Text = Name;
                codeLabel.Text = Code.ToString();
            }
        }
    }
}
