using BudgetUnderControl.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetUnderControl.Model.Services
{
    public interface ICategoryService
    {
        Task<ICollection<CategoryListItemDTO>> GetCategoriesAsync();
        Task<bool> IsValidAsync(int categoryId);
    }
}
