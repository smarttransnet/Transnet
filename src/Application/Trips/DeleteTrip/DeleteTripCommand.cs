using Application.Abstractions.Messaging;

namespace Application.Trips.DeleteTrip;

public sealed record DeleteTripCommand(Guid Id) : ICommand;
