using Application.Abstractions.Messaging;

namespace Application.VehicleCategories.UpdateVehicleCategory;

public sealed record UpdateVehicleCategoryCommand(Guid Id, string Name, string? Description, bool IsActive) : ICommand;
