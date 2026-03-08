using Application.Abstractions.Messaging;

namespace Application.Trips.ApproveTrip;

public sealed record ApproveTripCommand(Guid Id, Guid UserId) : ICommand;
