using Application.Abstractions.Messaging;
using Domain.Drivers;
using Domain.Drivers.Enums;
namespace Application.Drivers.Assignments.GetAssignments; public sealed record AssignmentResponse(Guid Id, Guid DriverId, Guid TripId, DateTime AssignedAt, Guid AssignedByUserId, DateTime? AcceptedAt, DateTime? RejectedAt, string? RejectionReason, AssignmentStatus AssignmentStatus, DateTime? DisplayedInAppAt);
