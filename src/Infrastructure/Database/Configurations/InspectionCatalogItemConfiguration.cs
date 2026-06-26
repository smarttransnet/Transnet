using Domain.Inspections;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configurations;

internal sealed class InspectionCatalogItemConfiguration : IEntityTypeConfiguration<InspectionCatalogItem>
{
    public void Configure(EntityTypeBuilder<InspectionCatalogItem> builder)
    {
        builder.ToTable("inspection_catalog_items", Schemas.Default);

        builder.HasKey(i => i.Id);

        builder.Property(i => i.Category)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(i => i.ItemName)
            .IsRequired()
            .HasMaxLength(255);
    }
}
