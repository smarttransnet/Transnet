using Application.Abstractions.Messaging;

namespace Application.Trips.DeleteTripPod;

public sealed record DeleteTripPodCommand(Guid Id, Guid TripId) : ICommand;
