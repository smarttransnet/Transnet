using Domain.Billing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configurations.Billing;

internal sealed class InvoicePaymentConfiguration : IEntityTypeConfiguration<InvoicePayment>
{
    public void Configure(EntityTypeBuilder<InvoicePayment> builder)
    {
        builder.ToTable("InvoicePayments");
        builder.HasKey(p => p.Id);

        builder.Property(p => p.AmountQAR).HasColumnType("decimal(18,2)");
        builder.Property(p => p.PaymentReference).HasMaxLength(150);
        builder.Property(p => p.Notes).HasMaxLength(1000);

        builder.HasIndex(p => p.InvoiceId);
        builder.HasIndex(p => p.PaymentDate);
    }
}
