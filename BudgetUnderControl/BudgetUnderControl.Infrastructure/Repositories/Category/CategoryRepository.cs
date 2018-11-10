using BudgetUnderControl.Common.Contracts;
using BudgetUnderControl.Domain;
using BudgetUnderControl.Domain.Repositiories;
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
        public CategoryRepository(IContextFacade context) : base(context)
        {
        }

        public async Task<ICollection<Category>> GetCategoriesAsync()
        {
            var list = await this.Context.Categories.ToListAsync();
            return list;
        }
    }
}
