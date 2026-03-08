using Domain.Billing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configurations.Billing;

internal sealed class InvoiceReportFormatConfiguration : IEntityTypeConfiguration<InvoiceReportFormat>
{
    public void Configure(EntityTypeBuilder<InvoiceReportFormat> builder)
    {
        builder.ToTable("InvoiceReportFormats");
        builder.HasKey(rf => rf.Id);

        builder.Property(rf => rf.Name).HasMaxLength(200).IsRequired();
        builder.Property(rf => rf.Description).HasMaxLength(1000);
        builder.Property(rf => rf.TemplateFileUrl).HasMaxLength(500);
        builder.Property(rf => rf.ColumnConfiguration).HasMaxLength(1500).IsRequired();
        builder.Property(rf => rf.HeaderLogoUrl).HasMaxLength(500);
        builder.Property(rf => rf.FooterText).HasMaxLength(2000);

        builder.HasIndex(rf => rf.Name);

        builder.HasMany(rf => rf.ReportColumns)
            .WithOne(c => c.ReportFormat)
            .HasForeignKey(c => c.InvoiceReportFormatId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
