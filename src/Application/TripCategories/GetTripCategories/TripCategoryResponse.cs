using System;
using System.Collections.Generic;

namespace Application.TripCategories.GetTripCategories;

public record UomDto(Guid MappingId, Guid UomId, string UomCode, string Description);

public record TripCategoryResponse(
    Guid CategoryId,
    string CategoryName,
    bool IsActive,
    List<UomDto> Uoms
);
