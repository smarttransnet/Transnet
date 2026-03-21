using Application.Abstractions.Messaging;

namespace Application.Assets.DeleteVehicle;

public sealed record DeleteVehicleCommand(Guid Id) : ICommand;
