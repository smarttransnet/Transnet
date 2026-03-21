using Application.Abstractions.Messaging;

namespace Application.Inspections.DeleteVehicleInspection;

public sealed record DeleteVehicleInspectionCommand(Guid Id) : ICommand;
