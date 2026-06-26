using SharedKernel;

namespace Domain.Inspections;

public sealed class InspectionCatalogItem : Entity
{
    public Guid Id { get; set; }
    public string Category { get; set; }
    public string ItemName { get; set; }
    public int SortOrder { get; set; }
    public bool IsActive { get; set; }
}
