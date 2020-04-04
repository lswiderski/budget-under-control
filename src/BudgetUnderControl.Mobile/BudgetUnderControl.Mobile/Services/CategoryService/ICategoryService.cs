using BudgetUnderControl.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetUnderControl.Mobile.Services
{
    public interface ICategoryService
    {
        Task<ICollection<CategoryListItemDTO>> GetCategoriesAsync();
        Task<CategoryListItemDTO> GetCategoryAsync(Guid id);
        Task<bool> IsValidAsync(int categoryId);
    }
}
