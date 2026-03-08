using SharedKernel;

namespace Domain.Drivers;

public sealed class DriverGpsLog : Entity
{
    public Guid Id { get; set; }
    public Guid DriverId { get; set; }
    public Guid? TripId { get; set; }
    public DateTime SessionStart { get; set; }
    public DateTime? SessionEnd { get; set; }
    public decimal? TotalDistanceKm { get; set; }
    public float? MaxSpeedKmh { get; set; }
    public int PointCount { get; set; }
    public string? RawTrackUrl { get; set; }

    // Navigation Properties
    public Driver Driver { get; set; }
    public ICollection<DriverLocationUpdate> LocationUpdates { get; set; } = [];
}
