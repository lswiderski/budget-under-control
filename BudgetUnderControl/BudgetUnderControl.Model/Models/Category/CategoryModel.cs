using BudgetUnderControl.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetUnderControl.Model
{
    public class CategoryModel : BaseModel, ICategoryModel
    {
        IContextConfig contextConfig;
        public CategoryModel(IContextConfig contextConfig) : base(contextConfig)
        {
            this.contextConfig = contextConfig;
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
