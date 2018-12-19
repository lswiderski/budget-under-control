using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BudgetUnderControl.Common.Contracts;
using BudgetUnderControl.Infrastructure.Commands;
using BudgetUnderControl.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BudgetUnderControl.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TagsController : ApiControllerBase
    {
        private ITagService tagService;

        public TagsController(ITagService accountService, ICommandDispatcher commandDispatcher) : base(commandDispatcher)
        {
            this.tagService = accountService;
        }

        // GET api/tags
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TagDTO>>> Get()
        {
            var accounts = await this.tagService.GetTagsAsync();
            return Ok(accounts.ToList());
        }

        // GET api/tags
        [HttpGet("active")]
        public async Task<ActionResult<IEnumerable<TagDTO>>> GetActive()
        {
            var accounts = await this.tagService.GetActiveTagsAsync();
            return Ok(accounts.ToList());
        }

        // GET api/tags/552cbd7c-e9d9-46c9-ab7e-2b10ae38ab4a
        [HttpGet("{id}")]
        public async Task<ActionResult<TagDTO>> GetTag(Guid id)
        {
            var account = await this.tagService.GetTagAsync(id);
            return Ok(account);
        }

        // POST api/tags
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] AddTag command)
        {
            await this.DispatchAsync(command);
            return CreatedAtAction(nameof(Get), new { id = command.ExternalId }, command);
        }

        // PUT api/tags/552cbd7c-e9d9-46c9-ab7e-2b10ae38ab4a
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] EditTag command)
        {
            await this.DispatchAsync(command);
            return NoContent();
        }
    }
}