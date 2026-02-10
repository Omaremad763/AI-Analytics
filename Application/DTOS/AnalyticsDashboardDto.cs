using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOS
{
    public record AnalyticsDashboardDto(
        decimal TotalIncome,
        decimal TotalExpense,
        decimal NetProfit,
        IEnumerable<DailyTransactionSummaryDto> DailySummaries
    );
}
