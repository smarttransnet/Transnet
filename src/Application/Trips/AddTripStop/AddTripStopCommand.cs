using Application.Abstractions.Messaging;
using Domain.Trips.Enums;

namespace Application.Trips.AddTripStop;

public sealed record AddTripStopCommand(
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
    string? Notes) : ICommand<Guid>;
