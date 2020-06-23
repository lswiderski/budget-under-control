using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BudgetUnderControl.CommonInfrastructure.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BudgetUnderControl.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ApiControllerBase
    {

        public FilesController(ICommandDispatcher commandDispatcher) : base(commandDispatcher)
        {
        }

        // GET api/files/552cbd7c-e9d9-46c9-ab7e-2b10ae38ab4a
        [HttpGet("{id}")]
        public async Task<ActionResult<Guid>> GetById(Guid id)
        {
            return Ok(id);
        }

        // POST api/files
        [HttpPost]
        public async Task<IActionResult> Post(IFormCollection files)
        {
            return CreatedAtAction(nameof(GetById), new { id = Guid.NewGuid() }) ;
        }

        // PUT api/files/552cbd7c-e9d9-46c9-ab7e-2b10ae38ab4a
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id)
        {
            return NoContent();
        }

        // DELETE api/files/552cbd7c-e9d9-46c9-ab7e-2b10ae38ab4a
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            return NoContent();
        }
    }
}