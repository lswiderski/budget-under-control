using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using BudgetUnderControl.Droid.PlatformSpecific;
using BudgetUnderControl.Mobile.CommonDTOs;
using BudgetUnderControl.Mobile.PlatformSpecific;
using Xamarin.Forms;

[assembly: Dependency(typeof(PhotoPickerService))]
namespace BudgetUnderControl.Droid.PlatformSpecific
{
    public class PhotoPickerService : IPhotoPickerService
    {
        public Task<ImagePickerResultDTO> GetImageStreamAsync()
        {
            Intent intent = new Intent();
            intent.SetType("image/*");
            intent.SetAction(Intent.ActionGetContent);

            MainActivity.Instance.StartActivityForResult(
                Intent.CreateChooser(intent, "Select Picture"),
                MainActivity.PickImageId);

            MainActivity.Instance.PickImageTaskCompletionSource = new TaskCompletionSource<ImagePickerResultDTO>();

            return MainActivity.Instance.PickImageTaskCompletionSource.Task;
        }
    }
}