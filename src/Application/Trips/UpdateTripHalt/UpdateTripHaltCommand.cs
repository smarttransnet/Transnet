using Application.Abstractions.Messaging;
using Domain.Trips.Enums;

namespace Application.Trips.UpdateTripHalt;

public sealed record UpdateTripHaltCommand(
    Guid TripId,
    Guid HaltId,
    HaltType HaltType,
    string? Reason,
    double? Latitude,
    double? Longitude,
    string? LocationName,
    DateTime StartedAt,
    DateTime? EndedAt) : ICommand;
