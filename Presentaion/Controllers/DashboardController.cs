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
    public async Task<IActionResult> GetChartss([FromQuery] DateTime start, [FromQuery] DateTime end)
    {
        Task<Application.DTOS.ChartDataDto>? trendTask = mediator.Send(new GetTrendDataQuery(start, end));
        var categoryTask = mediator.Send(new GetCategoryDistributionQuery());
        await Task.WhenAll(trendTask, categoryTask);
        var chartSummary = new
        {
            TrendData = trendTask.Result,
            CategoryDistribution = categoryTask.Result
        };

        var response = ApiResponse.Success(chartSummary);
        return Ok(response);
    }
}