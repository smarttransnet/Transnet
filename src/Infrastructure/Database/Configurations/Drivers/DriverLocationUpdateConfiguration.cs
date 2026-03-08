using Domain.Drivers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configurations.Drivers;

internal sealed class DriverLocationUpdateConfiguration : IEntityTypeConfiguration<DriverLocationUpdate>
{
    public void Configure(EntityTypeBuilder<DriverLocationUpdate> builder)
    {
        builder.ToTable("driver_location_updates");

        builder.HasKey(l => l.Id);

        builder.Property(l => l.DriverId).IsRequired();
        builder.HasIndex(l => l.DriverId);

        // Optional Trip link
        builder.Property(l => l.TripId);
        
        builder.Property(l => l.Latitude).IsRequired();
        builder.Property(l => l.Longitude).IsRequired();
        
        builder.Property(l => l.Accuracy);
        builder.Property(l => l.SpeedKmh);
        builder.Property(l => l.Bearing);
        
        builder.Property(l => l.RecordedAt).IsRequired();
        // Useful for getting latest locations
        builder.HasIndex(l => new { l.DriverId, l.RecordedAt });
        
        builder.Property(l => l.Source).IsRequired();
    }
}
