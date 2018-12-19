using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace BudgetUnderControl.Views
{
    public class TagListItem : ViewCell
    {
        Label nameLabel;

        public static readonly BindableProperty NameProperty = BindableProperty.Create("Name", typeof(string), typeof(TagListItem), "Name");

        public string Name
        {
            get { return (string)GetValue(NameProperty); }
            set { SetValue(NameProperty, value); }
        }

        public TagListItem()
        {
            var grid = new Grid { Padding = new Thickness(10) };
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            nameLabel = new Label { FontAttributes = FontAttributes.Bold };

            grid.Children.Add(nameLabel);


            View = grid;
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if (BindingContext != null)
            {
                nameLabel.Text = Name;
               
            }
        }
    }
}
