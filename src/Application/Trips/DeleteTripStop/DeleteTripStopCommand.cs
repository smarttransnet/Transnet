using Application.Abstractions.Messaging;

namespace Application.Trips.DeleteTripStop;

public sealed record DeleteTripStopCommand(Guid Id, Guid TripId) : ICommand;
