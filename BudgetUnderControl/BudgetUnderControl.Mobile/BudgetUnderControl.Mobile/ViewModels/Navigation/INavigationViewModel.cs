using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace BudgetUnderControl.Mobile
{
    public interface INavigationViewModel
    {
        bool IsLogged { get; set; }

        bool IsNotLogged { get; set; }

        List<MasterPageItem> MasterPageItems { get; set; }

        void RefreshUserButtons();
    }
}
