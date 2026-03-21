using Application.Abstractions.Messaging;

namespace Application.VehicleCategories.GetVehicleCategoryById;

public sealed record GetVehicleCategoryByIdQuery(Guid Id) : IQuery<VehicleCategoryResponse>;
