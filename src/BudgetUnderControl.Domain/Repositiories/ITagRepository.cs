using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BudgetUnderControl.Domain.Repositiories
{
    public interface ITagRepository
    {
        Task AddAsync(Tag tag);
        Task<ICollection<Tag>> GetAsync();
        Task<Tag> GetAsync(int id);
        Task<Tag> GetAsync(Guid id);
        Task UpdateAsync(Tag tag);
        Task RemoveAsync(Tag tag);

        Task AddAsync(TagToTransaction tagsToTransaction);
        Task<ICollection<TagToTransaction>> GetTagToTransactionAsync();
        Task<TagToTransaction> GetTagToTransactionAsync(int tagToTransactionId);        
        Task UpdateAsync(TagToTransaction tagsToTransaction);
        Task RemoveAsync(TagToTransaction tagsToTransaction);
    }
}
