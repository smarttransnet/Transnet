namespace Application.Fuel.Woqood.Queries.GetWoqoodBatchTransactions;

public sealed record WoqoodFuelTransactionResponse(
    Guid Id,
    string WoqoodCardNumber,
    Guid? VehicleId,
    Guid? DriverId,
    Guid? TripId,
    DateTime TransactionDate,
    string StationName,
    string FuelType,
    decimal QuantityLitres,
    decimal UnitPriceQAR,
    decimal TotalAmountQAR,
    decimal? Odometer,
    bool IsAllocated
);
