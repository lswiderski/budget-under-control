using Autofac;
using BudgetUnderControl.Mobile.ViewModels;
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
	public partial class EditTag : ContentPage
	{
        ITagViewModel vm;
        Guid tagId;
        public EditTag(Guid tagId)
        {
            InitializeComponent();
            using (var scope = App.Container.BeginLifetimeScope())
            {
                this.BindingContext = vm = scope.Resolve<ITagViewModel>();
            }
            this.tagId = tagId;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            vm.LoadTagAsync(tagId);
        }

        protected async void OnEditButtonClicked(object sender, EventArgs args)
        {
            await vm.EditTagAsync();
            App.MasterPage.NavigateTo(typeof(Tags));
        }
    }
}