using Application.Contracts;
using Application.Contracts.Dashboard;
using Application.DTOS;

using FluentValidation;

using MediatR;
namespace Application.CQRS;
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

public class DashboardHandler(IAI_AnalyticsServices service) :
    IRequestHandler<GetDashboardDataQuery, AnalyticsDashboardDto>,
    IRequestHandler<GetBatchDetailsQuery, IEnumerable<FinancialRecordDto>>
{
    public async Task<AnalyticsDashboardDto> Handle(GetDashboardDataQuery request, CancellationToken ct)
    {
        return await service.DashboardService.GetDashboardDataAsync(request.StartDate, request.EndDate);
    }

    public async Task<IEnumerable<FinancialRecordDto>> Handle(GetBatchDetailsQuery request, CancellationToken ct)
    {
        return await service.DashboardService.GetBatchDetailsAsync(request.BatchId);
    }
}