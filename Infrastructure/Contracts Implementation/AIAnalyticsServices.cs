

namespace Infrastructure.Contracts_Implementation;
using Application.Contracts;
using Application.Contracts.Dashboard;

using Infrastructure.Contracts_Implementation.Dashboard;
public class AIAnalyticsServices(
    IUnitofWork unitofWork
    ) : IAIAnalyticsServices
{
    public IDashboardService DashboardService => new DashboardService(unitofWork);
    public IExcelParserService ExcelParserService => new ExcelParserService();
}