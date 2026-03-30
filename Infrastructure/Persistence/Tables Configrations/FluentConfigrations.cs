using Domain.Entites;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence;

public class DataBatchConfiguration : IEntityTypeConfiguration<DataBatch>
{
    public void Configure(EntityTypeBuilder<DataBatch> builder)
    {
        builder.HasKey(b => b.Id);
        builder.Property(b => b.FileName).IsRequired().HasMaxLength(255);
        builder.Property(b => b.Status).HasConversion<string>();
        builder.Property(b => b.UploadedAt).HasDefaultValueSql("NOW() AT TIME ZONE 'UTC'");

        builder.HasMany(b => b.Records)
                   .WithOne(r => r.DataBatch)
                   .HasForeignKey(r => r.DataBatchId)
                   .OnDelete(DeleteBehavior.Cascade);
    }
}

public class FinancialRecordConfiguration : IEntityTypeConfiguration<FinancialRecord>
{
    public void Configure(EntityTypeBuilder<FinancialRecord> builder)
    {
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Description).IsRequired().HasMaxLength(500);
        builder.Property(r => r.Amount).HasPrecision(18, 2);
        builder.Property(r => r.Category).HasMaxLength(100);
        builder.Property(p => p.Type).HasConversion(v => v.ToString(), v => (FinacialReacordsEnum)Enum.Parse(typeof(FinacialReacordsEnum), v));
        builder.HasIndex(r => r.Category);
        builder.HasIndex(r => r.TransactionDate);
    }
}

public class BatchSummaryConfiguration : IEntityTypeConfiguration<BatchSummary>
{
    public void Configure(EntityTypeBuilder<BatchSummary> builder)
    {
        builder.HasKey(s => s.Id);
        builder.Property(s => s.AiInsightText).IsRequired();

        builder.HasOne(s => s.DataBatch)
               .WithOne(b => b.Summary)
               .HasForeignKey<BatchSummary>(s => s.DataBatchId);
    }
}