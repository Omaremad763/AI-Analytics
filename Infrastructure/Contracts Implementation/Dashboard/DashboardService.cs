using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Application.Contracts;
using Application.Contracts.Dashboard;
using Application.DTOS;

using AutoMapper;

using Domain.Entites;

namespace Infrastructure.Contracts_Implementation.Dashboard
{
    public class DashboardService( IUnitofWork unitOfWork,IMapper mapper) : IDashboardService
    {
        public async Task<AnalyticsDashboardDto> GetDashboardDataAsync(DateTime start, DateTime end)
        {
            var summary = await unitOfWork.DashboardRepository.GetFinancialSummaryAsync(start, end);
            var dailyData = await unitOfWork.DashboardRepository.GetDailySummariesAsync(start, end);
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
        public async Task SaveProcessedRecordsAsync(List<FinancialRecord> records)
        {
            await unitOfWork.DashboardRepository.BulkInsertRecordsAsync(records);
        }
        public async Task<IEnumerable<FinancialRecordDto>> GetBatchDetailsAsync(Guid batchId)
        {
            var records = await unitOfWork.DashboardRepository.GetRecordsByBatchIdAsync(batchId);
            return mapper.Map<IEnumerable<FinancialRecordDto>>(records);
        }
    }
}
