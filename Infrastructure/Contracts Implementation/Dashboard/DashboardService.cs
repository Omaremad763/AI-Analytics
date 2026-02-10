using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Application.Contracts.Dashboard;
using Application.DTOS;

using AutoMapper;

using Domain.Entites;

namespace Infrastructure.Contracts_Implementation.Dashboard
{
    public class DashboardService( IDashboardRepository repository,/*IUnitOfWork unitOfWork*/IMapper mapper) : IDashboardService
    {
        public async Task<AnalyticsDashboardDto> GetDashboardDataAsync(DateTime start, DateTime end)
        {
            var summary = await repository.GetFinancialSummaryAsync(start, end);
            var dailyData = await repository.GetDailySummariesAsync(start, end);
            var totalIncome = summary["Income"];
            var totalExpense = summary["Expense"];
            var netProfit = totalIncome - totalExpense;
            return new AnalyticsDashboardDto(
             totalIncome,
             totalExpense,
             netProfit,
             dailyData
         );

        }
        public async Task SaveProcessedRecordsAsync(IEnumerable<FinancialRecord> records)
        {
            await repository.BulkInsertRecordsAsync(records);

            //await unitOfWork.SaveChangesAsync();
        }
        public async Task<IEnumerable<FinancialRecordDto>> GetBatchDetailsAsync(Guid batchId)
        {
            var records = await repository.GetRecordsByBatchIdAsync(batchId);
            return mapper.Map<IEnumerable<FinancialRecordDto>>(records);
        }
    }
}
