using Domain.Assets.Enums;
using SharedKernel;

namespace Domain.Assets;

public sealed class Trailer : Entity
{
    public Guid Id { get; set; }
    public string TrailerNumber { get; set; }
    public string TrailerType { get; set; }
    public decimal Capacity { get; set; }
    public string CapacityUnit { get; set; }
    public Guid? AttachedVehicleId { get; set; }
    public TrailerStatus Status { get; set; }
    public Guid? CurrentLocationId { get; set; }
    public decimal TotalRevenueQAR { get; set; }
    public decimal TotalExpensesQAR { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public Vehicle? AttachedVehicle { get; set; }
}
