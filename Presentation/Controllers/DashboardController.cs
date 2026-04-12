using Application.CQRS;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DashboardController(IMediator mediator) : ControllerBase
{
    [HttpGet("metrics")]
    public async Task<IActionResult> GetMetrics()
    {
        var result = (await mediator.Send(new GetFinancialMetricsQuery()));
        var response = ApiResponse.Success(result);
        return Ok(response);
    }

    [HttpGet("charts")]
    public async Task<IActionResult> GetCharts([FromQuery] DateTime start, [FromQuery] DateTime end)
    {
        var  trendTask = await mediator.Send(new GetTrendDataQuery(start, end));
        IEnumerable<Application.DTOS.CategoryDistributionDto>? categoryTask = await mediator.Send(new GetCategoryDistributionQuery());
        var chartSummary = new
        {
            TrendData = trendTask,
            CategoryDistribution = categoryTask
        };

        var response = ApiResponse.Success(chartSummary);
        return Ok(response);
    }
}