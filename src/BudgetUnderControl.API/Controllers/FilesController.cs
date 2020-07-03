using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BudgetUnderControl.ApiInfrastructure.Services;
using BudgetUnderControl.CommonInfrastructure.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BudgetUnderControl.API.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ApiControllerBase
    {
        private readonly IWebHostEnvironment environment;
        private readonly IFileService fileService;

        public FilesController(ICommandDispatcher commandDispatcher, IWebHostEnvironment environment, IFileService fileService) : base(commandDispatcher)
        {
            this.environment = environment;
            this.fileService = fileService;
        }

        // GET api/files/552cbd7c-e9d9-46c9-ab7e-2b10ae38ab4a
        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(Guid id, string token)
        {
            if(string.IsNullOrWhiteSpace(token))
            {
                return BadRequest();
            }

            //TODO token validation

            var file = await this.fileService.GetFileAsync(id, token);
            
            if(file == null)
            {
                return NotFound();
            }
            return PhysicalFile(file.FilePath, file.ContentType, $"{file.Name}");
        }

        // POST api/files
        [HttpPost]
        [Authorize]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Post(IFormCollection files)
        {
            var file = files.Files.FirstOrDefault();

            if (file == null || file.Length == 0)
            {
                return BadRequest();
            }

            var fileId = await fileService.SaveFileAsync(file);

            return Ok(fileId);
        }

        // DELETE api/files/552cbd7c-e9d9-46c9-ab7e-2b10ae38ab4a
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(Guid id)
        {
            await fileService.RemoveFileAsync(id);
            return NoContent();
        }
    }
}