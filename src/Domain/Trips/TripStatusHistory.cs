using Domain.Trips.Enums;
using SharedKernel;

namespace Domain.Trips;

public sealed class TripStatusHistory : Entity
{
    public Guid Id { get; set; }
    public Guid TripId { get; set; }
    public TripStatus PreviousStatus { get; set; }
    public TripStatus NewStatus { get; set; }
    public Guid? ChangedByUserId { get; set; }
    public Guid? ChangedByDriverId { get; set; }
    public DateTime ChangedAt { get; set; }
    public string? Notes { get; set; }
    public string? AttachmentUrl { get; set; }
    public StatusChangeSource Source { get; set; }

    public Trip Trip { get; set; }
}
