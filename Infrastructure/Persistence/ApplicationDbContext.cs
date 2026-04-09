using Domain.Entites;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Infrastructure.Persistence
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        public DbSet<DataBatch> DataBatchs => Set<DataBatch>();
        public DbSet<FinancialRecord> FinancialRecords => Set<FinancialRecord>();
        public DbSet<BatchSummary> BatchSummary => Set<BatchSummary>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }
    }

    //factory pattern to create instance of ApplicationDbContext at design time for migrations
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var coonection = Environment.GetEnvironmentVariable("AiAnalyticsConnection");
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseNpgsql(coonection);
            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}