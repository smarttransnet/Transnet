using Domain.Assets;
using Domain.Drivers;
using Domain.Fuel.Enums;
using Domain.Trips;
using SharedKernel;

namespace Domain.Fuel;

public sealed class FuelCostAllocation : Entity
{
    public Guid Id { get; set; }
    public Guid? WoqoodFuelTransactionId { get; set; }
    public Guid? DriverExpenseId { get; set; }
    public Guid VehicleId { get; set; }
    public Guid? TripId { get; set; }
    public FuelAllocationSource AllocationSource { get; set; }
    public decimal QuantityLitres { get; set; }
    public decimal AmountQAR { get; set; }
    public DateOnly AllocationDate { get; set; }
    public Guid? AllocatedByUserId { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }

    // Navigation Properties
    public WoqoodFuelTransaction? WoqoodTransaction { get; set; }
    public DriverExpense? DriverExpense { get; set; }
    public Vehicle? Vehicle { get; set; }
    public Trip? Trip { get; set; }
}
