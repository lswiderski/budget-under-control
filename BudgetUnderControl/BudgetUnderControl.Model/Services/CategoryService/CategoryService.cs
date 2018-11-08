using BudgetUnderControl.Contracts.Models;
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
    }
}
