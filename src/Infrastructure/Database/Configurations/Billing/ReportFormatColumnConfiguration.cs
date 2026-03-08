using Domain.Billing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configurations.Billing;

internal sealed class ReportFormatColumnConfiguration : IEntityTypeConfiguration<ReportFormatColumn>
{
    public void Configure(EntityTypeBuilder<ReportFormatColumn> builder)
    {
        builder.ToTable("ReportFormatColumns");
        builder.HasKey(c => c.Id);

        builder.Property(c => c.ColumnKey).HasMaxLength(100).IsRequired();
        builder.Property(c => c.DisplayLabel).HasMaxLength(200).IsRequired();
        builder.Property(c => c.WidthPercent).HasColumnType("decimal(5,2)");
        builder.Property(c => c.FormatPattern).HasMaxLength(50);

        builder.HasIndex(c => c.InvoiceReportFormatId);
        builder.HasIndex(c => new { c.InvoiceReportFormatId, c.ColumnKey }).IsUnique();
    }
}
