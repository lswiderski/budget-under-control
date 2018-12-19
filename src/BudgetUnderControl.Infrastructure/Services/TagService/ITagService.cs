using BudgetUnderControl.Common.Contracts;
using BudgetUnderControl.Infrastructure.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BudgetUnderControl.Infrastructure.Services
{
    public interface ITagService
    {
        Task<ICollection<TagDTO>> GetTagsAsync();
        Task<ICollection<TagDTO>> GetActiveTagsAsync();
        Task<TagDTO> GetTagAsync(Guid tagId);
        Task AddTagAsync(AddTag command);
        Task EditTagAsync(EditTag command);
    }
}
