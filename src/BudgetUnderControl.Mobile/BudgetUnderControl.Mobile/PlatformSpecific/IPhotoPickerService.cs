using BudgetUnderControl.Mobile.CommonDTOs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace BudgetUnderControl.Mobile.PlatformSpecific
{
    public interface IPhotoPickerService
    {
        Task<ImagePickerResultDTO> GetImageStreamAsync();
    }
}
