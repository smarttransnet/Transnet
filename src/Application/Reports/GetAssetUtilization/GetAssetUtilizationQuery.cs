using Application.Abstractions.Messaging;

namespace Application.Reports.GetAssetUtilization;

public sealed record AssetUtilizationResponse(
    Guid VehicleId,
    string ChassisNumber,
    decimal TotalDistanceKm,
    int TotalTrips,
    int ActiveDays);

public sealed record GetAssetUtilizationQuery(DateTime StartDate, DateTime EndDate) : IQuery<List<AssetUtilizationResponse>>;
