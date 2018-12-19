using BudgetUnderControl.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BudgetUnderControl.Mobile.ViewModels
{
    public interface ITagViewModel
    {
        TagDTO SelectedTag { get; set; }
        ICollection<TagDTO> Tags { get; set; }
        string Name { get; set; }
        Task LoadTags();
        Task LoadTagAsync(Guid tagId);
        Task AddTagAsync();
        Task EditTagAsync();
    }
}
