using Domain.Drivers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configurations.Drivers;

internal sealed class DriverTripAssignmentConfiguration : IEntityTypeConfiguration<DriverTripAssignment>
{
    public void Configure(EntityTypeBuilder<DriverTripAssignment> builder)
    {
        builder.ToTable("driver_trip_assignments");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.DriverId).IsRequired();
        builder.HasIndex(a => a.DriverId);

        builder.Property(a => a.TripId).IsRequired();
        builder.HasIndex(a => a.TripId);

        builder.Property(a => a.AssignedAt).IsRequired();
        builder.Property(a => a.AssignedByUserId).IsRequired();
        
        builder.Property(a => a.AcceptedAt);
        builder.Property(a => a.RejectedAt);
        builder.Property(a => a.RejectionReason).HasMaxLength(1000);
        
        builder.Property(a => a.AssignmentStatus).IsRequired();
        builder.Property(a => a.DisplayedInAppAt);
    }
}
