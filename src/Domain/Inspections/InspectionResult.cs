using SharedKernel;

namespace Domain.Inspections;

public sealed class InspectionResult : Entity
{
    public Guid Id { get; set; }
    public Guid VehicleInspectionId { get; set; }
    public Guid ChecklistItemId { get; set; }
    public string Status { get; set; }
    public string? Remarks { get; set; }
    public DateTime RecordedAt { get; set; }

    public VehicleInspection VehicleInspection { get; set; }
    public ChecklistItem ChecklistItem { get; set; }
}
