using Domain.Entites;

using Infrastructure.views;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Persistence
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options
        ) : DbContext(options)
    {
        public DbSet<DataBatch> DataBatchs => Set<DataBatch>();
        public DbSet<FinancialRecord> FinancialRecords => Set<FinancialRecord>();
        public DbSet<BatchSummary> BatchSummary => Set<BatchSummary>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
            modelBuilder.Entity<ViewFinancialSummary>(eb => {
                eb.HasNoKey();
                eb.ToView("View_FinancialSummary");
            });
        }
    }

    public class ApplicationDbContextFactory() : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true)
            .AddEnvironmentVariables()
            .Build();
            var coonection = configuration["AiAnalyticsConnection"];
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseNpgsql(coonection);
            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}