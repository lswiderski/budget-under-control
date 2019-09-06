using Autofac;
using BudgetUnderControl.Mobile.Markers;
using BudgetUnderControl.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BudgetUnderControl.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddTransaction : ContentPage, ITagSelectablePage
    {
        IAddTransactionViewModel vm;
        public AddTransaction()
        {
            using (var scope = App.Container.BeginLifetimeScope())
            {
                this.BindingContext = vm = scope.Resolve<IAddTransactionViewModel>();
            }
            InitializeComponent();
            
            amount.Completed += (object sender, EventArgs e) => { name.Focus(); };
            name.Completed += (object sender, EventArgs e) => { type.Focus(); };
            type.Unfocused += (object sender, FocusEventArgs e) => { account.Focus(); };
            account.Unfocused += (object sender, FocusEventArgs e) => { categories.Focus(); };
        }

        public AddTransaction(string amount, string title) : this()
        {
            vm.Amount = amount;
            vm.Name = title;
        }

        async void OnAddButtonClicked(object sender, EventArgs args)
        {
            await vm.AddTransacionAsync();

            if(Navigation.ModalStack.Any())
            {
                await Navigation.PopModalAsync();
            }
            else
            {
                App.MasterPage.NavigateTo(typeof(Transactions));
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            amount.Focus();
        }

        async void OnSelectTagsButtonClicked(object sender, EventArgs args)
        {
            var selectTags = new SelectTags(this);
            await Navigation.PushModalAsync(selectTags);
        }

        async void OnDeleteTagButtonClicked(object sender, SelectedItemChangedEventArgs e)
        {
            Guid tagId = vm.SelectedTag.ExternalId;
            await vm.RemoveTagFromListAsync(tagId);
        }

        public async void AddTagToList(Guid tagId)
        {
            await vm.AddTagAsync(tagId);
        }

        async void OnGetLocationButtonClicked(object sender, System.EventArgs e)
        {
            try
            {
                var location = await Geolocation.GetLastKnownLocationAsync();

                if (location != null)
                {
                    vm.Latitude = location.Latitude;
                    vm.Longitude = location.Longitude;
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                await DisplayAlert("Faild", fnsEx.Message, "OK");
            }
            catch (PermissionException pEx)
            {
                await DisplayAlert("Faild", pEx.Message, "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Faild", ex.Message, "OK");
            }

            GetRealLocationAsync();
        }

        async void GetRealLocationAsync()
        {
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Medium);
                var location = await Geolocation.GetLocationAsync(request);

                if (location != null)
                {
                    vm.Latitude = location.Latitude;
                    vm.Longitude = location.Longitude;
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                await DisplayAlert("Faild", fnsEx.Message, "OK");
            }
            catch (FeatureNotEnabledException fneEx)
            {
                await DisplayAlert("Faild", fneEx.Message, "OK");
            }
            catch (PermissionException pEx)
            {
                await DisplayAlert("Faild", pEx.Message, "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Faild", ex.Message, "OK");
            }
        }

    }
}