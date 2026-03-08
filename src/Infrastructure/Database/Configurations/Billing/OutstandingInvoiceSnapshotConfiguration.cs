using Domain.Billing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configurations.Billing;

internal sealed class OutstandingInvoiceSnapshotConfiguration : IEntityTypeConfiguration<OutstandingInvoiceSnapshot>
{
    public void Configure(EntityTypeBuilder<OutstandingInvoiceSnapshot> builder)
    {
        builder.ToTable("OutstandingInvoiceSnapshots");
        builder.HasKey(s => s.Id);

        builder.Property(s => s.InvoiceNumber).HasMaxLength(50).IsRequired();
        builder.Property(s => s.OriginalAmountQAR).HasColumnType("decimal(18,2)");
        builder.Property(s => s.OutstandingAmountQAR).HasColumnType("decimal(18,2)");

        builder.HasIndex(s => s.OutstandingInvoiceReportId);

        builder.HasOne(s => s.Invoice)
            .WithMany()
            .HasForeignKey(s => s.InvoiceId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
