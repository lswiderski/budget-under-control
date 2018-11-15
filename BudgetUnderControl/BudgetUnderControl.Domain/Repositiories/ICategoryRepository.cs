using BudgetUnderControl.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetUnderControl.Domain.Repositiories
{
    public interface ICategoryRepository
    {
        Task<ICollection<Category>> GetCategoriesAsync();
        Task<ICollection<Category>> GetAllCategoriesAsync();
    }
}
