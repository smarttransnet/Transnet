using Application.Abstractions.Messaging;

namespace Application.VehicleCategories.DeleteVehicleCategory;

public sealed record DeleteVehicleCategoryCommand(Guid Id) : ICommand;
