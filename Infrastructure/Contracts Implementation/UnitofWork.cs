using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Application.Contracts;
using Application.Contracts.Dashboard;

using Infrastructure.Contracts_Implementation.Dashboard;
using Infrastructure.Persistence;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Contracts_Implementation
{
    public class UnitofWork(ApplicationDbContext context, IConfiguration configuration) : IUnitofWork
    {
        public IDashboardRepository DashboardRepository =>  new DashboardRepository(context, configuration);

        public IDataBatchRepository DataBatchRepository =>  new DataBatchRepository(context);

        public async Task<int> CommitAsync()
        {
            return await context.SaveChangesAsync();
        }

        public void Dispose()
        {
            context.Dispose();GC.SuppressFinalize(this);
        }
    }
}
