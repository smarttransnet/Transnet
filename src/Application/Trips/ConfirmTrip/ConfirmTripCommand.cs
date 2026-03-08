using Application.Abstractions.Messaging;

namespace Application.Trips.ConfirmTrip;

public sealed record ConfirmTripCommand(Guid Id, Guid DriverId) : ICommand;
