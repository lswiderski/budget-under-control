using Autofac;
using BudgetUnderControl.Mobile.Markers;
using BudgetUnderControl.Mobile.PlatformSpecific;
using BudgetUnderControl.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BudgetUnderControl.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditTransactionFile : ContentPage
    {
        IEditTransactionViewModel vm;
        public EditTransactionFile()
        {
            InitializeComponent();
        }

        public void SetContext(IEditTransactionViewModel model)
        {
            this.BindingContext = vm = model;
        }

        async void OnAddImageButtonClicked(object sender, EventArgs e)
        {
            (sender as Button).IsEnabled = false;

            var pickerResult = await DependencyService.Get<IPhotoPickerService>().GetImageStreamAsync();
            Stream stream = pickerResult?.Stream;
            if (stream != null)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    stream.CopyTo(ms);
                    vm.ImageByteArray = ms.ToArray();
                }

                Stream stream2 = new MemoryStream(vm.ImageByteArray);
                vm.HasImage = true;
                vm.HasNoImage = false;
                stream2.Position = 0;
                vm.ImageSource = ImageSource.FromStream(() => stream2);
            }
            (sender as Button).IsEnabled = true;
        }

        async void OnDeleteImageButtonClicked(object sender, EventArgs e)
        {
            (sender as Button).IsEnabled = false;

            vm.ImageSource = null;
            vm.ImageByteArray = null;
            vm.HasImage = false;
            vm.HasNoImage = true;
            (sender as Button).IsEnabled = true;
        }
    }
}