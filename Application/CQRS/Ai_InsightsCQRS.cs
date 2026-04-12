using Application.Contracts.Ai_Insights;
using Application.DTOS;

using FluentValidation;

using MediatR;

namespace Application.CQRS;
public record GetFinancialInsightsQuery(DateTime start, DateTime end) : IRequest<AIInsightDto>;

public class GetFinancialInsightsValidator : AbstractValidator<GetFinancialInsightsQuery>
{
    public GetFinancialInsightsValidator()
    {
        RuleFor(x => x.start).NotEmpty();
        RuleFor(x => x.end).NotEmpty();
        RuleFor(x => x).Must(x => x.end >= x.start)
            .WithMessage("End date must be bigger than start date");
    }
}

public class AIInsightsHandler(IAIInsightService aiService) : IRequestHandler<GetFinancialInsightsQuery, AIInsightDto>
{
    public async Task<AIInsightDto> Handle(GetFinancialInsightsQuery request, CancellationToken cancellationToken)
    {
        return await aiService.GetAIInsightReportAsync(request.start, request.end);
    }
}