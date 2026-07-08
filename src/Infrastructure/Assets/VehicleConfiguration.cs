using Domain.Assets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Assets;

internal sealed class VehicleConfiguration : IEntityTypeConfiguration<Vehicle>
{
    public void Configure(EntityTypeBuilder<Vehicle> builder)
    {
        builder.HasKey(v => v.Id);
        
        builder.Property(v => v.PlateNumber).IsRequired().HasMaxLength(50);
        builder.Property(v => v.ChassisNumber).HasMaxLength(100);
        builder.Property(v => v.Make).IsRequired().HasMaxLength(100);
        builder.Property(v => v.Model).IsRequired().HasMaxLength(100);
        
        builder.Property(v => v.CreatedAt).HasConversion(d => DateTime.SpecifyKind(d, DateTimeKind.Utc), v => v);
        builder.Property(v => v.UpdatedAt).HasConversion(d => DateTime.SpecifyKind(d, DateTimeKind.Utc), v => v);

        builder.HasOne(v => v.Category)
            .WithMany(c => c.Vehicles)
            .HasForeignKey(v => v.VehicleCategoryId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
