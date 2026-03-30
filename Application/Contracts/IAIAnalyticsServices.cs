using Application.Contracts.Dashboard;

namespace Application.Contracts;

public interface IAIAnalyticsServices
{
    public IDashboardService DashboardService { get; }
    public IExcelParserService ExcelParserService { get; }
}