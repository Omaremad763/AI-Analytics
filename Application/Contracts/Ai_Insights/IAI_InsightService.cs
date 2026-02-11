using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Application.DTOS;

namespace Application.Contracts.Ai_Insights
{
    public interface IAI_InsightService
    {
        Task<AIInsightDto> GetAIInsightReportAsync(DateTime start, DateTime end);
    }
}
