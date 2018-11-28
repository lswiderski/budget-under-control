using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BudgetUnderControl.Common.Contracts;
using BudgetUnderControl.Infrastructure.Commands;
using BudgetUnderControl.Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BudgetUnderControl.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SyncController : ApiControllerBase
    {
        private readonly ISyncService syncService;
        public SyncController(ISyncService syncService, ICommandDispatcher commandDispatcher) : base(commandDispatcher)
        {
            this.syncService = syncService;
        }
        // GET api/sync/export
        [HttpGet("backup")]
        public async Task<ActionResult<BackUpDTO>> Export()
        {
            var backup = await this.syncService.GetBackUpAsync();
            return Ok(backup);
        }

        // POST api/sync/import
        [HttpPost("backup")]
        public async Task<IActionResult> Import([FromBody] BackUpDTO json)
        {
            await this.syncService.ImportBackUpAsync(json);
            return Ok();
        }

        // GET api/sync/csv
        [HttpGet("csv")]
        public async Task<ContentResult> GenerateCSV()
        {
            var csv = await this.syncService.GenerateCSV();
            return Content(string.Join(System.Environment.NewLine, csv));
        }

        // POST api/sync/sync
        [HttpPost("sync")]
        public async Task<IActionResult> Sync([FromBody] SyncRequest request)
        {
            //await this.DispatchAsync(request);
            //temporary no CQRS

            var response = await this.syncService.SyncAsync(request);
            return Ok(response);
        }
    }
}