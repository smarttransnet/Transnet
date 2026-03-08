using Application.Abstractions.Messaging;

namespace Application.Trips.UpdateTripHaltEnd;

public sealed record UpdateTripHaltEndCommand(Guid Id, Guid TripId, DateTime EndedAt) : ICommand;
