using Application.Contracts;
using Application.DTOS;

using FluentValidation;

using MediatR;

namespace Application.CQRS;


public record GetFinancialMetricsQuery() : IRequest<IEnumerable<MetricCardDto>>;

public record GetCategoryDistributionQuery() : IRequest<IEnumerable<CategoryDistributionDto>>;

public record GetTrendDataQuery(DateTime StartDate, DateTime EndDate) : IRequest<ChartDataDto>;

public class GetTrendDataValidator : AbstractValidator<GetTrendDataQuery>
{
    public GetTrendDataValidator()
    {
        RuleFor(x => x.StartDate).NotEmpty();
        RuleFor(x => x.EndDate).NotEmpty();
    }
}


public class DashboardHandler(IAIAnalyticsServices service) :
    IRequestHandler<GetFinancialMetricsQuery, IEnumerable<MetricCardDto>>,
    IRequestHandler<GetCategoryDistributionQuery, IEnumerable<CategoryDistributionDto>>,
    IRequestHandler<GetTrendDataQuery, ChartDataDto>
{
    public async Task<IEnumerable<MetricCardDto>> Handle(GetFinancialMetricsQuery request, CancellationToken cancellationToken)
    {
        return await service.DashboardService.GetMetricCardsAsync();
    }

    public async Task<IEnumerable<CategoryDistributionDto>> Handle(GetCategoryDistributionQuery request, CancellationToken cancellationToken)
    {
        return await service.DashboardService.GetCategoryDataAsync();
    }

    public async Task<ChartDataDto> Handle(GetTrendDataQuery request, CancellationToken cancellationToken)
    {
        return await service.DashboardService.GetDailyTrendChartAsync(request.StartDate, request.EndDate);
    }
}