using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xamarin.Forms.PlatformConfiguration;

namespace BudgetUnderControl.Mobile.CommonDTOs
{
    public class ImagePickerResultDTO
    {
        public Stream Stream { get; set; }

        public string Path { get; set; }
    }
}
