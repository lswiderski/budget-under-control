using BudgetUnderControl.Common.Contracts;
using BudgetUnderControl.MobileDomain;
using BudgetUnderControl.MobileDomain.Repositiories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BudgetUnderControl.CommonInfrastructure;

namespace BudgetUnderControl.Mobile.Repositories
{
    public class CategoryRepository : BaseModel, ICategoryRepository
    {
        private readonly IUserIdentityContext userIdentityContext;

        public CategoryRepository(IContextFacade context, IUserIdentityContext userIdentityContext) : base(context)
        {
            this.userIdentityContext = userIdentityContext;
        }

        public async Task<ICollection<Category>> GetCategoriesAsync()
        {
            var list = await this.Context.Categories.Where(x => x.OwnerId == userIdentityContext.UserId).ToListAsync();
            return list;
        }

        public async Task<ICollection<Category>> GetAllCategoriesAsync()
        {
            var list = await this.Context.Categories.ToListAsync();
            return list;
        }

        public async Task<Category> GetCategoryAsync(Guid id)
        {
            var category = await this.Context.Categories.Where(x => x.ExternalId == id).FirstOrDefaultAsync();
            return category;
        }

        public async Task<Category> GetCategoryAsync(string name)
        {
            var category = await this.Context.Categories.Where(x => x.Name == name).FirstOrDefaultAsync();
            return category;
        }

        public async Task UpdateAsync(Category category)
        {
            category.UpdateModify();
            this.Context.Categories.Update(category);
            await this.Context.SaveChangesAsync();
        }

        public async Task AddCategoryAsync(Category category)
        {
            this.Context.Categories.Add(category);
            await this.Context.SaveChangesAsync();
        }
    }
}
