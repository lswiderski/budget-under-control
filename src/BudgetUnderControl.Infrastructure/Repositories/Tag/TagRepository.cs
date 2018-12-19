using BudgetUnderControl.Domain;
using BudgetUnderControl.Domain.Repositiories;
using BudgetUnderControl.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetUnderControl.Infrastructure.Repositories
{
    public class TagRepository : BaseModel, ITagRepository
    {
        private readonly IUserIdentityContext userIdentityContext;

        public TagRepository(IContextFacade context, IUserIdentityContext userIdentityContext) : base(context)
        {
            this.userIdentityContext = userIdentityContext;
        }

        public async Task AddAsync(Tag tag)
        {
            this.Context.Tags.Add(tag);
            await this.Context.SaveChangesAsync();
        }

        public async Task<ICollection<Tag>> GetAsync()
        {
            var list = await this.Context.Tags
                .Where(t => t.OwnerId == userIdentityContext.UserId)
                .OrderByDescending(t => t.ModifiedOn)
                .ToListAsync();

            return list;
        }

        public async Task<Tag> GetAsync(int id)
        {
            var tag = await this.Context.Tags
                .Where(t => t.OwnerId == userIdentityContext.UserId)
                .FirstOrDefaultAsync(x => x.Id == id);

            return tag;
        }

        public async Task<Tag> GetAsync(Guid id)
        {
            var tag = await this.Context.Tags.Where(t => t.OwnerId == userIdentityContext.UserId).FirstOrDefaultAsync(x => x.ExternalId == id);

            return tag;
        }

        public async Task UpdateAsync(Tag tag)
        {
            this.Context.Tags.Update(tag);
            await this.Context.SaveChangesAsync();
        }

        public async Task RemoveAsync(Tag tag)
        {
            this.Context.Tags.Add(tag);
            await this.Context.SaveChangesAsync();
        }

        public async Task<TagToTransaction> GetTagToTransactionAsync(int tagToTransactionId)
        {
            var tagToTransaction = await this.Context.TagsToTransactions.FirstOrDefaultAsync(x => x.Id == tagToTransactionId);
            return tagToTransaction;
        }

        public async Task<ICollection<TagToTransaction>> GetTagToTransactionAsync()
        {
            var list = await this.Context.TagsToTransactions.ToListAsync();
            return list;
        }

        public async Task AddAsync(TagToTransaction tagsToTransaction)
        {
            this.Context.TagsToTransactions.Add(tagsToTransaction);
            await this.Context.SaveChangesAsync();
        }

        public async Task UpdateAsync(TagToTransaction tagsToTransaction)
        {
            this.Context.TagsToTransactions.Update(tagsToTransaction);
            await this.Context.SaveChangesAsync();
        }

        public async Task RemoveAsync(TagToTransaction tagsToTransaction)
        {
            this.Context.TagsToTransactions.Add(tagsToTransaction);
            await this.Context.SaveChangesAsync();
        }
    }
}
