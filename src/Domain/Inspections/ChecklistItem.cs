using SharedKernel;

namespace Domain.Inspections;

public sealed class ChecklistItem : Entity
{
    public Guid Id { get; set; }
    public Guid InspectionChecklistId { get; set; }
    public string ItemName { get; set; }
    public string Category { get; set; }
    public bool IsRequired { get; set; }
    public int SortOrder { get; set; }

    public InspectionChecklist InspectionChecklist { get; set; }
}
