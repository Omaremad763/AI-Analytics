using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Application.Contracts;
using Application.Contracts.Ai_Insights;
using Application.Contracts.Dashboard;
using Application.Contracts.Data_Ingestion;

using AutoMapper;

using Hangfire;

using Infrastructure.Contracts_Implementation.Dashboard;

using Microsoft.Extensions.Configuration;

namespace Infrastructure.Contracts_Implementation
{
    public class AI_AnalyticsServices(IMapper mapper, HttpClient httpClient, IConfiguration configuration
        , IUnitofWork unitofWork,
        IBackgroundJobClient backgroundJobClient,
        IAI_AnalyticsServices service
        ) : IAI_AnalyticsServices
    {
        public IDashboardService DashboardService =>  new DashboardService(unitofWork,mapper);

        public IData_InegstionService Data_InegstionService =>  new Data_InegstionService(unitofWork, backgroundJobClient,service,mapper);

        public IExcelParserService ExcelParserService =>  new ExcelParserService();
    }
}
