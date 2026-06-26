using Application.Abstractions.Messaging;
using Domain.Trips.Enums;

namespace Application.Trips.TransitionTripStatus;

public sealed record TransitionTripStatusCommand(
    Guid Id,
    TripStatus NewStatus,
    string? Notes,
    StatusChangeSource Source,
    Guid? ChangedByUserId,
    Guid? ChangedByDriverId,
    Stream? PhotoStream = null,
    string? PhotoFileName = null) : ICommand;
