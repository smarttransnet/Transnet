using Application.Abstractions.Messaging;
using Domain.Trips.Enums;

namespace Application.Trips.RecordTripHalt;

public sealed record RecordTripHaltCommand(
    Guid TripId,
    HaltType HaltType,
    string? Reason,
    double? Latitude,
    double? Longitude,
    string? LocationName,
    DateTime StartedAt,
    Guid RecordedByDriverId) : ICommand<Guid>;
