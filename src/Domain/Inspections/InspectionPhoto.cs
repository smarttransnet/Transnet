using Domain.Inspections.Enums;
using SharedKernel;

namespace Domain.Inspections;

public sealed class InspectionPhoto : Entity
{
    public Guid Id { get; set; }
    public Guid VehicleInspectionId { get; set; }
    public string PhotoPath { get; set; }
    public string? Caption { get; set; }
    public PhotoType PhotoType { get; set; }
    public DateTime UploadedAt { get; set; }
    public Guid UploadedByDriverId { get; set; }

    public VehicleInspection VehicleInspection { get; set; }
}
