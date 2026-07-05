using SharedKernel;
using Domain.Assets;

namespace Domain.Trips;

public sealed class VehicleCategoryUom : Entity
{
    public Guid Id { get; set; }
    public Guid VehicleCategoryId { get; set; }

    public Guid UOMId { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedDate { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public Guid? ModifiedBy { get; set; }

    // Navigation properties
    public VehicleCategory? VehicleCategory { get; set; }

    public Uom? Uom { get; set; }
}
