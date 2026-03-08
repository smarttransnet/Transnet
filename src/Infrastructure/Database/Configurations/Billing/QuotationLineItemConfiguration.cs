using Domain.Billing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configurations.Billing;

internal sealed class QuotationLineItemConfiguration : IEntityTypeConfiguration<QuotationLineItem>
{
    public void Configure(EntityTypeBuilder<QuotationLineItem> builder)
    {
        builder.ToTable("QuotationLineItems");
        builder.HasKey(li => li.Id);

        builder.Property(li => li.Description).HasMaxLength(500).IsRequired();
        builder.Property(li => li.Quantity).HasColumnType("decimal(18,2)");
        builder.Property(li => li.UnitPriceQAR).HasColumnType("decimal(18,2)");
        builder.Property(li => li.DiscountPercent).HasColumnType("decimal(5,2)");
        builder.Property(li => li.TaxPercent).HasColumnType("decimal(5,2)");
        builder.Property(li => li.LineTotalQAR).HasColumnType("decimal(18,2)");

        builder.HasIndex(li => li.QuotationId);
    }
}
