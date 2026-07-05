using SharedKernel;

namespace Domain.Trips;

public sealed class Uom : Entity
{
    public Guid Id { get; set; }
    public string UOMCode { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public ICollection<VehicleCategoryUom> VehicleCategoryUoms { get; set; } = [];
}
