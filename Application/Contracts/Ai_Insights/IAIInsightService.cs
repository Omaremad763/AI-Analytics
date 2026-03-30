using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Application.DTOS;

namespace Application.Contracts.Ai_Insights
{
    public interface IAIInsightService
    {
        Task<AIInsightDto> GetAIInsightReportAsync(DateTime start, DateTime end);
    }
}
