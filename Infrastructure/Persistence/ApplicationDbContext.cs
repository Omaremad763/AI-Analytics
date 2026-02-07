using System.Linq.Expressions;

using Domain.Entites;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Infrastructure.Persistence
{
    public class ApplicationDbContext: DbContext
    {
        public DbSet<DataBatch> DataBatchs => Set<DataBatch>();
        public DbSet<FinancialRecord> FinancialRecords => Set<FinancialRecord>();
        public DbSet<BatchSummary> BatchSummary => Set<BatchSummary>();

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

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

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseNpgsql("Host=localhost; Port=5432; Database=AI-Analytics; Username=postgres; Password=123456Oo#;");
            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }

}
