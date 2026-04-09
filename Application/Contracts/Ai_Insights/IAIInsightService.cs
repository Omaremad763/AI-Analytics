using Application.DTOS;

namespace Application.Contracts.Ai_Insights
{
    public interface IAIInsightService
    {
        Task<AIInsightDto> GetAIInsightReportAsync(DateTime start, DateTime end);
    }
}