namespace Application.VehicleCategories;

public sealed record VehicleCategoryResponse(Guid Id, string Name, string? Description, bool IsActive);
