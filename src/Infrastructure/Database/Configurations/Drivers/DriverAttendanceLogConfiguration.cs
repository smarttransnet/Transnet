using Domain.Drivers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configurations.Drivers;

internal sealed class DriverAttendanceLogConfiguration : IEntityTypeConfiguration<DriverAttendanceLog>
{
    public void Configure(EntityTypeBuilder<DriverAttendanceLog> builder)
    {
        builder.ToTable("driver_attendance_logs");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.DriverId).IsRequired();
        builder.HasIndex(a => a.DriverId);

        builder.Property(a => a.AttendanceDate).IsRequired();
        // Composite index for querying a driver's attendance on a specific date
        builder.HasIndex(a => new { a.DriverId, a.AttendanceDate });

        builder.Property(a => a.CheckInAt);
        builder.Property(a => a.CheckOutAt);
        
        builder.Property(a => a.CheckInLatitude);
        builder.Property(a => a.CheckInLongitude);
        builder.Property(a => a.CheckOutLatitude);
        builder.Property(a => a.CheckOutLongitude);
        
        builder.Property(a => a.TotalHoursWorked).HasPrecision(5, 2);
        
        builder.Property(a => a.Notes).HasMaxLength(500);
        builder.Property(a => a.Source).IsRequired();
    }
}
