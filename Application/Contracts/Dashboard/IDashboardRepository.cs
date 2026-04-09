using Application.DTOS;

using Domain.Entites;

namespace Application.Contracts.Dashboard
{
    public interface IDashboardRepository
    {
        Task BulkInsertRecordsAsync(IEnumerable<FinancialRecord> records);

        Task<AnalyticsDashboardDto> GetFinancialCardMetricsAsync();

        Task<IEnumerable<DailyTransactionSummaryDto>> GetDailyChartsAsync(DateTime startDate, DateTime endDate);

        Task<IEnumerable<CategoryDistributionDto>> GetCategoryDistributionAsync();

        Task<IEnumerable<FinancialRecord>> GetRecordsByBatchIdAsync(Guid batchId);
    }
}