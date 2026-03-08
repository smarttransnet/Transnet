using Application.Abstractions.Messaging;

namespace Application.VehicleCategories.GetVehicleCategories;

public sealed record VehicleCategoryResponse(Guid Id, string Name, string? Description, bool IsActive);

public sealed record GetVehicleCategoriesQuery : IQuery<List<VehicleCategoryResponse>>;
