using BudgetUnderControl.Common.Contracts;
using BudgetUnderControl.Domain;
using BudgetUnderControl.Domain.Repositiories;
using BudgetUnderControl.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetUnderControl.Model
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
    }
}
