using Domain.Trips;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configurations.Trips;

internal sealed class UomConfiguration : IEntityTypeConfiguration<Uom>
{
    public void Configure(EntityTypeBuilder<Uom> builder)
    {
        builder.ToTable("uoms");
        builder.HasKey(u => u.Id);
        builder.Property(u => u.UOMCode).IsRequired().HasMaxLength(20);
        builder.HasIndex(u => u.UOMCode).IsUnique();
        builder.Property(u => u.Description).HasMaxLength(250);
        builder.Property(u => u.IsActive).IsRequired().HasDefaultValue(true);

        builder.HasMany(u => u.VehicleCategoryUoms)
            .WithOne(cm => cm.Uom)
            .HasForeignKey(cm => cm.UOMId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
