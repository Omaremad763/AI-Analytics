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
        public async Task<List<MetricCardDto>> GetMetricCardsAsync()
        {

            var summary = await unitOfWork.DashboardRepository.GetFinancialCardMetricsAsync();
            return new List<MetricCardDto>
        {
            new("Total Income", summary.TotalIncome, "trending_up", "#4CAF50"),
            new("Total Expenses", summary.TotalExpense, "trending_down", "#F44336"),
            new("Net Profit", summary.NetProfit, "account_balance_wallet", "#2196F3")
        };
        }
        public async Task<ChartDataDto> GetDailyTrendChartAsync(DateTime start, DateTime end)
        {
            var dailyData = await unitOfWork.DashboardRepository.GetDailyChartsAsync(start, end);
            return new ChartDataDto
            {
                Labels = dailyData.Select(d => d.TransactionDate.ToString("MMM dd")).ToList(),
                Datasets = new List<ChartDatasetDto>
            {
                new ChartDatasetDto
                {
                    Label = "Daily Transactions",
                    Data = dailyData.Select(d => d.TotalAmount).ToList(),
                    BackgroundColor = "rgba(33, 150, 243, 0.2)",
                    BorderColor = "#2196F3"
                }
            }
            };
        }
        public async Task<IEnumerable<CategoryDistributionDto>> GetCategoryDataAsync()
        {
            return await unitOfWork.DashboardRepository.GetCategoryDistributionAsync();
        }
        public async Task BulkInsertRecordsAsync(IEnumerable<FinancialRecord> records)
        {
             await unitOfWork.DashboardRepository.BulkInsertRecordsAsync(records);
        }
    }
}

