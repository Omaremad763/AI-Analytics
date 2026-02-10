using System.Data;

using Application.Contracts.Dashboard;
using Application.DTOS;

using Dapper;

using Domain.Entites;

using Infrastructure.Persistence;

using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

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

        public async Task<Dictionary<string, decimal>> GetFinancialSummaryAsync(DateTime startDate, DateTime endDate)
        {
            var data = await context.FinancialRecords
                .AsNoTracking()
                .Where(r => r.TransactionDate >= startDate && r.TransactionDate <= endDate)
                .GroupBy(r => 1)
                .Select(g => new
                {
                    TotalIncome = g.Where(x => x.Amount > 0).Sum(x => x.Amount),
                    TotalExpense = g.Where(x => x.Amount < 0).Sum(x => x.Amount)
                })
                .FirstOrDefaultAsync();

            return new Dictionary<string, decimal>
        {
            { "Income", data?.TotalIncome ?? 0 },
            { "Expense", Math.Abs(data?.TotalExpense ?? 0) }
        };
        }
        public async Task<IEnumerable<DailyTransactionSummaryDto>> GetDailySummariesAsync(DateTime startDate, DateTime endDate)
        {
            using IDbConnection db = new SqlConnection(connectionString);

            // SQL مباشر وبسيط، Dapper بيحوله لـ Objects بسرعة خارقة
            var sql = @"SELECT TransactionDate AS Date, TotalAmount, TransactionCount 
                        FROM View_FinancialDailySummaries 
                        WHERE TransactionDate BETWEEN @Start AND @End 
                        ORDER BY TransactionDate";

            return await db.QueryAsync<DailyTransactionSummaryDto>(sql, new { Start = startDate, End = endDate });
        }
}



