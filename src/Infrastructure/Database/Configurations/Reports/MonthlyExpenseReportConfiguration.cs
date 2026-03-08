using Domain.Reports;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configurations.Reports;

internal sealed class MonthlyExpenseReportConfiguration : IEntityTypeConfiguration<MonthlyExpenseReport>
{
    public void Configure(EntityTypeBuilder<MonthlyExpenseReport> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.TotalFuelCostQAR).HasPrecision(18, 2);
        builder.Property(x => x.TotalSalaryCostQAR).HasPrecision(18, 2);
        builder.Property(x => x.TotalDriverExpensesQAR).HasPrecision(18, 2);
        builder.Property(x => x.TotalOperationalCostQAR).HasPrecision(18, 2);
        builder.Property(x => x.ExportedFileUrl).HasMaxLength(1000);
        builder.Property(x => x.Notes).HasMaxLength(1000);

        builder.HasMany(x => x.LineItems)
            .WithOne(l => l.Report)
            .HasForeignKey(l => l.MonthlyExpenseReportId)
            .OnDelete(DeleteBehavior.Cascade);
            
        builder.HasIndex(x => new { x.PeriodYear, x.PeriodMonth, x.ReportType }).IsUnique();
    }
}
