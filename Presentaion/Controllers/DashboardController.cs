using Application.CQRS;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController(IMediator mediator) : ControllerBase
    {
        [HttpGet("dashboard")]
        public async Task<IActionResult> GetDashboard([FromQuery] DateTime start, [FromQuery] DateTime end)
        {
            return Ok(await mediator.Send(new GetDashboardDataQuery(start, end)));
        }
        [HttpGet("batch/{id:guid}")]
        public async Task<IActionResult> GetBatchDetails(Guid id)
        {
            return Ok(await mediator.Send(new GetBatchDetailsQuery(id)));
        }
    }

