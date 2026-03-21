using Application.Abstractions.Messaging;
using Application.VehicleCategories;

namespace Application.VehicleCategories.GetVehicleCategories;

public sealed record GetVehicleCategoriesQuery : IQuery<IReadOnlyList<VehicleCategoryResponse>>;
