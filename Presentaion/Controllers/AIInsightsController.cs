using Application.CQRS;

using MediatR;

using Microsoft.AspNetCore.Mvc;
namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AIInsightsController(IMediator mediator) : ControllerBase
    {
        [HttpGet("analyze")]
        public async Task<IActionResult> GetInsights([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var result = await mediator.Send(new GetFinancialInsightsQuery(startDate, endDate));
            return Ok(result);
        }
    }
}
