using BudgetUnderControl.Common.Contracts;
using BudgetUnderControl.Domain.Repositiories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetUnderControl.Model.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }

        public async Task<ICollection<CategoryListItemDTO>> GetCategoriesAsync()
        {
            var categories = await this.categoryRepository.GetCategoriesAsync();

            var dtos = categories.Select(x => new CategoryListItemDTO
            {
                Id = x.Id,
                Name = x.Name
            })
               .ToList();

            return dtos;
        }

        public async Task<bool> IsValidAsync(int categoryId)
        {
            var currencies = await this.categoryRepository.GetCategoriesAsync();
            return currencies.Any(x => x.Id == categoryId);
        }
    }
}
