using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetUnderControl.Mobile.Markers
{
    public interface ITagSelectablePage
    {
        void AddTagToList(Guid tagId);
    }
}
