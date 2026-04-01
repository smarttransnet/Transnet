using Domain.Inspections.Enums;
using SharedKernel;

namespace Domain.Inspections;

public sealed class InspectionChecklist : Entity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Guid? VehicleCategoryId { get; set; }
    public InspectionType InspectionType { get; set; }
    public string ApplicableVehicleTypes { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }

    public ICollection<ChecklistItem> Items { get; set; } = [];
}
