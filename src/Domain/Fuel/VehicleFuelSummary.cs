using Domain.Assets;
using SharedKernel;

namespace Domain.Fuel;

public sealed class VehicleFuelSummary : Entity
{
    public Guid Id { get; set; }
    public Guid VehicleId { get; set; }
    public int PeriodMonth { get; set; }
    public int PeriodYear { get; set; }
    public decimal TotalLitres { get; set; }
    public decimal TotalCostQAR { get; set; }
    public decimal AverageCostPerLitreQAR { get; set; }
    public decimal? AverageFuelEfficiencyKmPerL { get; set; }
    public int WoqoodTransactionCount { get; set; }
    public int DriverEntryCount { get; set; }
    public DateTime LastUpdatedAt { get; set; }

    // Navigation Properties
    public Vehicle? Vehicle { get; set; }
}
