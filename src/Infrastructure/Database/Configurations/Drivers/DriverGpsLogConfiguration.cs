using Domain.Drivers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configurations.Drivers;

internal sealed class DriverGpsLogConfiguration : IEntityTypeConfiguration<DriverGpsLog>
{
    public void Configure(EntityTypeBuilder<DriverGpsLog> builder)
    {
        builder.ToTable("driver_gps_logs");

        builder.HasKey(g => g.Id);

        builder.Property(g => g.DriverId).IsRequired();
        builder.HasIndex(g => g.DriverId);

        builder.Property(g => g.TripId);
        
        builder.Property(g => g.SessionStart).IsRequired();
        builder.Property(g => g.SessionEnd);
        
        builder.Property(g => g.TotalDistanceKm).HasPrecision(18, 2);
        builder.Property(g => g.MaxSpeedKmh);
        builder.Property(g => g.PointCount).IsRequired().HasDefaultValue(0);
        
        builder.Property(g => g.RawTrackUrl).HasMaxLength(2048);
    }
}
