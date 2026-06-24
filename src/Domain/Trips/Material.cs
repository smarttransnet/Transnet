using SharedKernel;

namespace Domain.Trips;

public sealed class Material : Entity
{
    public Guid Id { get; set; }
    public Guid TripCategoryId { get; set; }
    public string MaterialName { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public DateTime CreatedDate { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public Guid? ModifiedBy { get; set; }

    // Navigation properties
    public TripCategory? TripCategory { get; set; }
    public ICollection<TripCategoryMaterial> CategoryMaterials { get; set; } = [];
}
