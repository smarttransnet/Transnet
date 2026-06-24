using SharedKernel;

namespace Domain.Trips;

public sealed class TripCategoryMaterial : Entity
{
    public Guid Id { get; set; }
    public Guid TripCategoryId { get; set; }
    public Guid MaterialId { get; set; }
    public Guid UOMId { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedDate { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public Guid? ModifiedBy { get; set; }

    // Navigation properties
    public TripCategory? TripCategory { get; set; }
    public Material? Material { get; set; }
    public Uom? Uom { get; set; }
}
