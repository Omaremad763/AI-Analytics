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

namespace Infrastructure.Contracts_Implementation;

    public class AIAnalyticsServices( 
        IUnitofWork unitofWork
        ) : IAIAnalyticsServices
    {
        public IDashboardService DashboardService =>  new DashboardService(unitofWork);
        public IExcelParserService ExcelParserService =>  new ExcelParserService();
    }

