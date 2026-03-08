using Domain.Trips.Enums;

namespace Application.Trips.Common;

public sealed record TripHaltResponse(
    Guid Id,
    Guid TripId,
    HaltType HaltType,
    string? Reason,
    double? Latitude,
    double? Longitude,
    string? LocationName,
    DateTime StartedAt,
    DateTime? EndedAt,
    int? DurationMinutes,
    Guid RecordedByDriverId);
