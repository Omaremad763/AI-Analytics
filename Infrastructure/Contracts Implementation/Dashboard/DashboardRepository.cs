using System.Data;

using Application.Contracts.Dashboard;
using Application.DTOS;

using Dapper;

using Domain.Entites;

using Infrastructure.Persistence;
using Infrastructure.views;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

using Npgsql;

namespace Infrastructure.Contracts_Implementation.Dashboard;

public class DashboardRepository(ApplicationDbContext context, 
    IConfiguration configuration) 
    : IDashboardRepository
{
    private readonly string connectionString = configuration["AiAnalyticsConnection"];

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
            .Select(g => new
            {
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
        return await context.Set<ViewFinancialSummary>()
            .AsNoTracking()         
            .Where(x => x.TransactionDate >= startDate && x.TransactionDate <= endDate)
            .Select(x => new DailyTransactionSummaryDto
            {
                TransactionDate = x.TransactionDate,
                TotalAmount = x.TotalAmount,
                TransactionCount = x.TransactionCount
            })
            .ToListAsync();
    }
    public async Task<IEnumerable<CategoryDistributionDto>> GetCategoryDistributionAsync()
    {
        var data = await context.FinancialRecords
            .GroupBy(x => x.Category)
            .Select(group => new
            {
                Category = group.Key,
                Total = group.Sum(x => x.Amount)
            })
            .ToListAsync();
        var totalAll = data.Sum(x => x.Total);
        return data
            .Select(x => new CategoryDistributionDto
            {
                CategoryName = x.Category,
                TotalAmount = x.Total,
                Percentage = totalAll > 0
                    ? (double)(x.Total / totalAll * 100)
                    : 0
            })
            .OrderByDescending(x => x.TotalAmount);
    }
}