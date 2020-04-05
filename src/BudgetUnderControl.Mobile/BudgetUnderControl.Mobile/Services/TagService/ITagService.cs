using BudgetUnderControl.Common.Contracts;
using BudgetUnderControl.CommonInfrastructure.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BudgetUnderControl.Mobile.Services
{
    public interface ITagService
    {
        Task<ICollection<TagDTO>> GetTagsAsync();
        Task<ICollection<TagDTO>> GetActiveTagsAsync();
        Task<TagDTO> GetTagAsync(string tagId);
        Task AddTagAsync(AddTag command);
        Task EditTagAsync(EditTag command);
    }
}
