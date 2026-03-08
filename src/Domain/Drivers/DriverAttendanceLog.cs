using Domain.Drivers.Enums;
using SharedKernel;

namespace Domain.Drivers;

public sealed class DriverAttendanceLog : Entity
{
    public Guid Id { get; set; }
    public Guid DriverId { get; set; }
    public DateOnly AttendanceDate { get; set; }
    public DateTime? CheckInAt { get; set; }
    public DateTime? CheckOutAt { get; set; }
    public double? CheckInLatitude { get; set; }
    public double? CheckInLongitude { get; set; }
    public double? CheckOutLatitude { get; set; }
    public double? CheckOutLongitude { get; set; }
    public decimal? TotalHoursWorked { get; set; }
    public string? Notes { get; set; }
    public AttendanceSource Source { get; set; }

    // Navigation Property
    public Driver Driver { get; set; }
}
