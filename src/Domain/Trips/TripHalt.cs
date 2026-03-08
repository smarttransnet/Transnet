using Domain.Trips.Enums;
using SharedKernel;

namespace Domain.Trips;

public sealed class TripHalt : Entity
{
    public Guid Id { get; set; }
    public Guid TripId { get; set; }
    public HaltType HaltType { get; set; }
    public string? Reason { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public string? LocationName { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime? EndedAt { get; set; }
    public int? DurationMinutes { get; set; }
    public Guid RecordedByDriverId { get; set; }

    public Trip Trip { get; set; }
}
