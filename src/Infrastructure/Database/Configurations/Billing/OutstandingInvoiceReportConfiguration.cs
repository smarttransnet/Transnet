using Domain.Billing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configurations.Billing;

internal sealed class OutstandingInvoiceReportConfiguration : IEntityTypeConfiguration<OutstandingInvoiceReport>
{
    public void Configure(EntityTypeBuilder<OutstandingInvoiceReport> builder)
    {
        builder.ToTable("OutstandingInvoiceReports");
        builder.HasKey(r => r.Id);

        builder.Property(r => r.TotalOutstandingQAR).HasColumnType("decimal(18,2)");
        builder.Property(r => r.SentToEmail).HasMaxLength(500);
        builder.Property(r => r.ExportedFileUrl).HasMaxLength(500);

        builder.HasIndex(r => r.ClientId);
        builder.HasIndex(r => new { r.PeriodYear, r.PeriodMonth });

        builder.HasMany(r => r.Snapshots)
            .WithOne(s => s.Report)
            .HasForeignKey(s => s.OutstandingInvoiceReportId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
