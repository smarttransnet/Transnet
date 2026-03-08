using Domain.Billing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configurations.Billing;

internal sealed class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
{
    public void Configure(EntityTypeBuilder<Invoice> builder)
    {
        builder.ToTable("Invoices");
        builder.HasKey(i => i.Id);

        builder.Property(i => i.InvoiceNumber).HasMaxLength(50).IsRequired();
        builder.Property(i => i.SubTotalQAR).HasColumnType("decimal(18,2)");
        builder.Property(i => i.TaxAmountQAR).HasColumnType("decimal(18,2)");
        builder.Property(i => i.TotalQAR).HasColumnType("decimal(18,2)");
        builder.Property(i => i.PaidAmountQAR).HasColumnType("decimal(18,2)");
        builder.Property(i => i.OutstandingAmountQAR).HasColumnType("decimal(18,2)");
        builder.Property(i => i.Notes).HasMaxLength(1500);

        builder.HasIndex(i => i.InvoiceNumber).IsUnique();
        builder.HasIndex(i => i.ClientId);

        builder.HasMany(i => i.LineItems)
            .WithOne(li => li.Invoice)
            .HasForeignKey(li => li.InvoiceId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(i => i.Payments)
            .WithOne(p => p.Invoice)
            .HasForeignKey(p => p.InvoiceId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(i => i.TripLinks)
            .WithOne(tl => tl.Invoice)
            .HasForeignKey(tl => tl.InvoiceId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(i => i.ReportFormat)
            .WithMany(rf => rf.Invoices)
            .HasForeignKey(i => i.ReportFormatId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(i => i.ReminderLogs)
            .WithOne(rl => rl.Invoice)
            .HasForeignKey(rl => rl.InvoiceId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
