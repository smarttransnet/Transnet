using Domain.Fuel.Enums;
using SharedKernel;

namespace Domain.Fuel;

public sealed class WoqoodFuelTransaction : Entity
{
    public Guid Id { get; set; }
    public Guid WoqoodImportBatchId { get; set; }
    public string WoqoodCardNumber { get; set; } = string.Empty;
    public Guid? VehicleId { get; set; }
    public Guid? DriverId { get; set; }
    public Guid? TripId { get; set; }
    public DateTime TransactionDate { get; set; }
    public string StationName { get; set; } = string.Empty;
    public FuelType FuelType { get; set; }
    public decimal QuantityLitres { get; set; }
    public decimal UnitPriceQAR { get; set; }
    public decimal TotalAmountQAR { get; set; }
    public decimal? Odometer { get; set; }
    public bool IsAllocated { get; set; }
    public string? RawRowData { get; set; }

    // Navigation Properties
    public WoqoodImportBatch? ImportBatch { get; set; }
    public FuelCostAllocation? Allocation { get; set; }
}
