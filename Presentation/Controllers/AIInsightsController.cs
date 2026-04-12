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
        public async Task<IActionResult> GetInsights([FromQuery] DateTime start, [FromQuery] DateTime end)
        {
            var result = await mediator.Send(new GetFinancialInsightsQuery(start, end));
            var response = ApiResponse.Success(result);
            return Ok(response);
        }
    }
}