using Domain.Assets;
using Domain.Inspections.Enums;
using SharedKernel;

namespace Domain.Inspections;

public sealed class VehicleInspection : Entity
{
    public Guid Id { get; set; }
    public Guid VehicleId { get; set; }
    public Guid InspectionChecklistId { get; set; }
    public InspectionType InspectionType { get; set; }
    public Guid DriverId { get; set; }
    public Guid? TripId { get; set; }
    public DateTime InspectedAt { get; set; }
    public string? DriverSignature { get; set; }
    public DateTime? DriverSignedAt { get; set; }
    public string? Notes { get; set; }
    public decimal OdometerReading { get; set; }
    public InspectionStatus Status { get; set; }

    public Vehicle Vehicle { get; set; }
    public InspectionChecklist InspectionChecklist { get; set; }

    public ICollection<InspectionResult> InspectionResults { get; set; } = [];
    public ICollection<InspectionPhoto> Photos { get; set; } = [];
}
