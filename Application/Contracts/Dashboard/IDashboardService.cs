using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Application.DTOS;

using Domain.Entites;

namespace Application.Contracts.Dashboard
{
    public interface IDashboardService
    {
        Task<AnalyticsDashboardDto> GetDashboardDataAsync(DateTime start, DateTime end);

        Task SaveProcessedRecordsAsync(List<FinancialRecord> records);

        Task<IEnumerable<FinancialRecordDto>> GetBatchDetailsAsync(Guid batchId);
    }
}
