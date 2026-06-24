using SharedKernel;

namespace Domain.Trips;

public sealed class TripCategory : Entity
{
    public Guid Id { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public DateTime CreatedDate { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public Guid? ModifiedBy { get; set; }

    // Navigation properties
    public ICollection<Material> Materials { get; set; } = [];
    public ICollection<TripCategoryMaterial> CategoryMaterials { get; set; } = [];
}
