using System.Text.Json;

using Application.Contracts.Ai_Insights;
using Application.DTOS;

using FluentValidation;

using MediatR;
namespace Application.CQRS;
public record GetFinancialInsightsQuery(DateTime StartDate, DateTime EndDate): IRequest<AIInsightDto>;
public class GetFinancialInsightsValidator : AbstractValidator<GetFinancialInsightsQuery>
{
    public GetFinancialInsightsValidator()
    {
        RuleFor(x => x.StartDate).NotEmpty();
        RuleFor(x => x.EndDate).NotEmpty();
        RuleFor(x => x).Must(x => x.EndDate >= x.StartDate)
            .WithMessage("تاريخ النهاية يجب أن يكون بعد تاريخ البداية للحصول على تحليل دقيق.");
    }
}
public class AIInsightsHandler( IAI_InsightService aiService) :IRequestHandler<GetFinancialInsightsQuery, AIInsightDto>
{
    public async Task<AIInsightDto> Handle(GetFinancialInsightsQuery request, CancellationToken ct)
    {

        return await aiService.GetAIInsightReportAsync(request.StartDate,request.EndDate);
    }
}

