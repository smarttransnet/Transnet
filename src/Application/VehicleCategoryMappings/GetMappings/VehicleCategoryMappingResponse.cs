using System;
using System.Collections.Generic;

namespace Application.VehicleCategoryMappings.GetMappings;

public sealed record VehicleCategoryMappingResponse(
    Guid CategoryId,
    string CategoryName,
    bool IsActive,
    List<VehicleCategoryUomResponse> Uoms
);
