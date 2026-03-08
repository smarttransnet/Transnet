using Domain.Assets.Enums;

namespace Application.Trailers.GetTrailers;

public sealed record TrailerResponse(
    Guid Id,
    string TrailerNumber,
    string TrailerType,
    decimal Capacity,
    string CapacityUnit,
    Guid? AttachedVehicleId,
    TrailerStatus Status,
    decimal TotalRevenueQAR,
    decimal TotalExpensesQAR,
    bool IsActive);
