using Domain.Trips;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configurations.Trips;

internal sealed class VehicleCategoryUomConfiguration : IEntityTypeConfiguration<VehicleCategoryUom>
{
    public void Configure(EntityTypeBuilder<VehicleCategoryUom> builder)
    {
        builder.ToTable("vehicle_category_uoms");
        builder.HasKey(cm => cm.Id);
        builder.Property(cm => cm.IsActive).IsRequired().HasDefaultValue(true);
        builder.Property(cm => cm.CreatedDate).IsRequired();
        builder.Property(cm => cm.CreatedBy).IsRequired();

        // Unique constraint for active mapping
        builder.HasIndex(cm => new { cm.VehicleCategoryId, cm.UOMId })
            .IsUnique()
            .HasFilter("is_active = true");
    }
}
