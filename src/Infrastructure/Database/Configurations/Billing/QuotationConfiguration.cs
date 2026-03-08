using Domain.Billing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configurations.Billing;

internal sealed class QuotationConfiguration : IEntityTypeConfiguration<Quotation>
{
    public void Configure(EntityTypeBuilder<Quotation> builder)
    {
        builder.ToTable("Quotations");
        builder.HasKey(q => q.Id);

        builder.Property(q => q.QuotationNumber).HasMaxLength(50).IsRequired();
        builder.Property(q => q.SubTotalQAR).HasColumnType("decimal(18,2)");
        builder.Property(q => q.TaxAmountQAR).HasColumnType("decimal(18,2)");
        builder.Property(q => q.TotalQAR).HasColumnType("decimal(18,2)");
        builder.Property(q => q.Notes).HasMaxLength(1000);
        builder.Property(q => q.TermsAndConditions).HasMaxLength(2000);

        builder.HasIndex(q => q.QuotationNumber).IsUnique();
        builder.HasIndex(q => q.ClientId);

        builder.HasMany(q => q.LineItems)
            .WithOne(li => li.Quotation)
            .HasForeignKey(li => li.QuotationId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(q => q.ConvertedInvoice)
            .WithOne(i => i.Quotation)
            .HasForeignKey<Invoice>(i => i.QuotationId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
