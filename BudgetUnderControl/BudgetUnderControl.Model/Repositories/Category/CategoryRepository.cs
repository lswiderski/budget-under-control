using BudgetUnderControl.Contracts.Models;
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

        public async Task<ICollection<CategoryListItemDTO>> GetCategories()
        {
            var list = this.Context.Categories
                .Select( x=> new CategoryListItemDTO
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .ToListAsync();

            return await list;
        }
    }
}
