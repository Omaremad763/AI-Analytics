using System.Data;

using Application.Contracts.Dashboard;
using Application.DTOS;

using Dapper;

using Domain.Entites;

using Infrastructure.Persistence;

using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;

using Npgsql;

namespace Infrastructure.Contracts_Implementation.Dashboard;

    public class DashboardRepository(ApplicationDbContext context, IConfiguration configuration) : IDashboardRepository
    {
        readonly string connectionString = configuration.GetConnectionString("DefaultConnection");
        public async Task BulkInsertRecordsAsync(IEnumerable<FinancialRecord> records)
        {
            await context.FinancialRecords.AddRangeAsync(records);
        }
        public async Task<IEnumerable<FinancialRecord>> GetRecordsByBatchIdAsync(Guid batchId)
        {
            return await context.FinancialRecords
                .AsNoTracking()
                .Where(r => r.Id == batchId)
                .ToListAsync();
        }
        public async Task<AnalyticsDashboardDto> GetFinancialCardMetricsAsync()
    {

        var stats = await context.FinancialRecords
            .AsNoTracking()
            .GroupBy(r => r.Type)
            .Select(g => new {
                Type = g.Key,
                Total = g.Sum(r => r.Amount)
            })
            .ToListAsync();

        var income = stats.FirstOrDefault(x => x.Type == FinacialReacordsEnum.Income)?.Total ?? 0;
        var expense = stats.FirstOrDefault(x => x.Type == FinacialReacordsEnum.Expense)?.Total ?? 0;
        var net = income - Math.Abs(expense);
        return new AnalyticsDashboardDto(income, expense, net, []);
    }
        public async Task<IEnumerable<DailyTransactionSummaryDto>> GetDailyChartsAsync(DateTime startDate, DateTime endDate)
        {
            using NpgsqlConnection db = new(connectionString);
            var Postgres = @"SELECT ""TransactionDate"" AS ""Date"", ""TotalAmount"", ""TransactionCount""
                 FROM public.""View_FinancialDailySummaries""";
             return await db.QueryAsync<DailyTransactionSummaryDto>(Postgres, new { Start = startDate, End = endDate });
        }
        public async Task<IEnumerable<CategoryDistributionDto>> GetCategoryDistributionAsync()
        {
            var totalAll = await context.FinancialRecords.SumAsync(x => x.Amount);
            return await context.FinancialRecords
                .GroupBy(x => x.Category)
                .Select(group => new CategoryDistributionDto
                {
                    CategoryName = group.Key,
                    TotalAmount = group.Sum(x => x.Amount),
                    Percentage = totalAll > 0
                                 ? (double)(group.Sum(x => x.Amount) / totalAll * 100)
                                 : 0
                })
                .OrderByDescending(x => x.TotalAmount)
                .ToListAsync();
        }

}



