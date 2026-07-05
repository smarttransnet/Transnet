using SharedKernel;
using Domain.Trips;

namespace Domain.Assets;

public sealed class VehicleCategory : Entity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }

    public ICollection<Vehicle> Vehicles { get; set; } = [];
    public ICollection<VehicleCategoryUom> VehicleCategoryUoms { get; set; } = [];
}
