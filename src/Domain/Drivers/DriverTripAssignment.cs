using Domain.Drivers.Enums;
using SharedKernel;

namespace Domain.Drivers;

public sealed class DriverTripAssignment : Entity
{
    public Guid Id { get; set; }
    public Guid DriverId { get; set; }
    public Guid TripId { get; set; }
    public DateTime AssignedAt { get; set; }
    public Guid AssignedByUserId { get; set; }
    public DateTime? AcceptedAt { get; set; }
    public DateTime? RejectedAt { get; set; }
    public string? RejectionReason { get; set; }
    public AssignmentStatus AssignmentStatus { get; set; }
    public DateTime? DisplayedInAppAt { get; set; }

    // Navigation Property
    public Driver Driver { get; set; }
}
