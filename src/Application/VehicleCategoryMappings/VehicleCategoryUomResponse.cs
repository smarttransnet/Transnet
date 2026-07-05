using System;

namespace Application.VehicleCategoryMappings;

public sealed record VehicleCategoryUomResponse(
    Guid MappingId,
    Guid UomId,
    string UomCode,
    string? Description
);
