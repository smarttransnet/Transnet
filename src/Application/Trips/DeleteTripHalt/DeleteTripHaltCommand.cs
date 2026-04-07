using Application.Abstractions.Messaging;

namespace Application.Trips.DeleteTripHalt;

public sealed record DeleteTripHaltCommand(Guid TripId, Guid HaltId) : ICommand;
