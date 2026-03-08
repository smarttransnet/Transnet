namespace Application.Fuel.Summaries.Queries.GetVehicleFuelSummaries;

public sealed record VehicleFuelSummaryResponse(
    Guid Id,
    Guid VehicleId,
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
