using Domain.Trips;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configurations.Trips;

internal sealed class TripCategoryMaterialConfiguration : IEntityTypeConfiguration<TripCategoryMaterial>
{
    public void Configure(EntityTypeBuilder<TripCategoryMaterial> builder)
    {
        builder.ToTable("trip_category_materials");
        builder.HasKey(cm => cm.Id);
        builder.Property(cm => cm.IsActive).IsRequired().HasDefaultValue(true);
        builder.Property(cm => cm.CreatedDate).IsRequired();
        builder.Property(cm => cm.CreatedBy).IsRequired();

        // Unique constraint for active mapping
        builder.HasIndex(cm => new { cm.TripCategoryId, cm.UOMId })
            .IsUnique()
            .HasFilter("is_active = true");
    }
}
