using Application.Contracts.Dashboard;
using Application.DTOS;
using FluentValidation;
using MediatR;
namespace Application.Features.Analytics;
public record GetDashboardDataQuery(DateTime StartDate, DateTime EndDate):IRequest<AnalyticsDashboardDto>;
public record GetBatchDetailsQuery(Guid BatchId): IRequest<IEnumerable<FinancialRecordDto>>;
public class GetDashboardDataValidator : AbstractValidator<GetDashboardDataQuery>
{
    public GetDashboardDataValidator()
    {
        RuleFor(x => x.StartDate).NotEmpty();
        RuleFor(x => x.EndDate).NotEmpty();
        RuleFor(x => x).Must(x => x.EndDate >= x.StartDate)
            .WithMessage("تاريخ النهاية يجب أن يكون بعد تاريخ البداية");
    }
}

public class GetBatchDetailsValidator : AbstractValidator<GetBatchDetailsQuery>
{
    public GetBatchDetailsValidator()
    {
        RuleFor(x => x.BatchId).NotEmpty().WithMessage("معرف الدفعة مطلوب");
    }
}

public class AnalyticsHandler(IDashboardService DashboardService) :
    IRequestHandler<GetDashboardDataQuery, AnalyticsDashboardDto>,
    IRequestHandler<GetBatchDetailsQuery, IEnumerable<FinancialRecordDto>>
{
    public async Task<AnalyticsDashboardDto> Handle(GetDashboardDataQuery request, CancellationToken ct)
    {
        return await DashboardService.GetDashboardDataAsync(request.StartDate, request.EndDate);
    }

    public async Task<IEnumerable<FinancialRecordDto>> Handle(GetBatchDetailsQuery request, CancellationToken ct)
    {
        return await DashboardService.GetBatchDetailsAsync(request.BatchId);
    }
}