using Domain.Trips.Enums;

namespace Application.Trips.Common;

public sealed record TripStopResponse(
    Guid Id,
    Guid TripId,
    int StopOrder,
    StopType StopType,
    string LocationName,
    string? Address,
    double? Latitude,
    double? Longitude,
    string? PocName,
    string? PocPhone,
    string? PocEmail,
    DateTime? ScheduledArrivalAt,
    DateTime? ActualArrivalAt,
    DateTime? ActualDepartureAt,
    string? Notes);
