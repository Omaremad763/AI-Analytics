using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Application.Contracts.Ai_Insights;
using Application.Contracts.Dashboard;
using Application.Contracts.Data_Ingestion;

namespace Application.Contracts;
public interface IAI_AnalyticsServices
{
    public IDashboardService DashboardService { get; }
    public IExcelParserService ExcelParserService { get; }
}
