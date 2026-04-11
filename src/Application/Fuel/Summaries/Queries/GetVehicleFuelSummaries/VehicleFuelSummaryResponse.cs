namespace Application.Fuel.Summaries.Queries.GetVehicleFuelSummaries;

public sealed record VehicleFuelSummaryResponse(
    Guid Id,
    Guid VehicleId,
    string VehiclePlate,
    int PeriodMonth,
    int PeriodYear,
    decimal TotalLitres,
    decimal TotalCostQAR,
    decimal AverageCostPerLitreQAR,
    decimal? AverageFuelEfficiencyKmPerL,
    int WoqoodTransactionCount,
    int DriverEntryCount,
    DateTime LastUpdatedAt
);
