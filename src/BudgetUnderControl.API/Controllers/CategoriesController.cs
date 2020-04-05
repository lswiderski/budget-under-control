using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BudgetUnderControl.Common.Contracts;
using BudgetUnderControl.CommonInfrastructure.Commands;
using BudgetUnderControl.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BudgetUnderControl.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ApiControllerBase
    {
        private readonly ICategoryService categoryService;

        public CategoriesController(ICategoryService categoryService, ICommandDispatcher commandDispatcher) : base(commandDispatcher)
        {
            this.categoryService = categoryService;
        }

        // GET api/categories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryListItemDTO>>> Get()
        {
            var categories = await this.categoryService.GetCategoriesAsync();
            return Ok(categories.ToList());
        }

        // GET api/categories/cb80cf16-02db-47ba-9ee7-5d9e78a695db
        [HttpGet("{id}")]
        public async Task<ActionResult<EditTransactionDTO>> GetById(Guid id)
        {
            var category = await this.categoryService.GetCategoryAsync(id);
            return Ok(category);
        }

    }
}