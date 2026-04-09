using Application.DTOS;

using Domain.Entites;

namespace Application.Contracts.Dashboard
{
    public interface IDashboardService
    {
        Task BulkInsertRecordsAsync(IEnumerable<FinancialRecord> records);

        Task<List<MetricCardDto>> GetMetricCardsAsync();

        Task<ChartDataDto> GetDailyTrendChartAsync(DateTime start, DateTime end);

        Task<IEnumerable<CategoryDistributionDto>> GetCategoryDataAsync();
    }
}