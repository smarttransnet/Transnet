using Domain.Drivers.Enums;
using SharedKernel;

namespace Domain.Drivers;

public sealed class DriverLocationUpdate : Entity
{
    public Guid Id { get; set; }
    public Guid DriverId { get; set; }
    public Guid? TripId { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public float? Accuracy { get; set; }
    public float? SpeedKmh { get; set; }
    public float? Bearing { get; set; }
    public DateTime RecordedAt { get; set; }
    public LocationSource Source { get; set; }

    // Navigation Property
    public Driver Driver { get; set; }
}
