using Application.Abstractions.Messaging;

namespace Application.VehicleCategories.CreateVehicleCategory;

public sealed record CreateVehicleCategoryCommand(string Name, string? Description) : ICommand<Guid>;
