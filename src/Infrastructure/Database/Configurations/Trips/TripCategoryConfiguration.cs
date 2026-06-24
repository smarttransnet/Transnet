using Domain.Trips;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configurations.Trips;

internal sealed class TripCategoryConfiguration : IEntityTypeConfiguration<TripCategory>
{
    public void Configure(EntityTypeBuilder<TripCategory> builder)
    {
        builder.ToTable("trip_categories");
        builder.HasKey(t => t.Id);
        builder.Property(t => t.CategoryName).IsRequired().HasMaxLength(150);
        builder.Property(t => t.IsActive).IsRequired().HasDefaultValue(true);
        builder.Property(t => t.CreatedDate).IsRequired();
        builder.Property(t => t.CreatedBy).IsRequired();
        
        builder.HasMany(t => t.Materials)
            .WithOne(m => m.TripCategory)
            .HasForeignKey(m => m.TripCategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(t => t.CategoryMaterials)
            .WithOne(cm => cm.TripCategory)
            .HasForeignKey(cm => cm.TripCategoryId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
