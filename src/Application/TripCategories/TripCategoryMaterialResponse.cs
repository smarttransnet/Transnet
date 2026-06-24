using System;

namespace Application.TripCategories;

public sealed record TripCategoryMaterialResponse(
    Guid Id,
    Guid TripCategoryId,
    string CategoryName,
    Guid MaterialId,
    string MaterialName,
    Guid UomId,
    string UomCode,
    bool IsActive
);
