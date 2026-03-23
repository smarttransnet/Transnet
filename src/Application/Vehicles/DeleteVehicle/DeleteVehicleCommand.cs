using Application.Abstractions.Messaging;

namespace Application.Vehicles.DeleteVehicle;

public sealed record DeleteVehicleCommand(Guid Id) : ICommand;
