using Domain.Trips.Enums;

namespace Application.Trips.Common;

public sealed record TripStatusHistoryResponse(
    Guid Id,
    Guid TripId,
    TripStatus PreviousStatus,
    TripStatus NewStatus,
    Guid? ChangedByUserId,
    Guid? ChangedByDriverId,
    DateTime ChangedAt,
    string? Notes,
    StatusChangeSource Source);
