using Domain.Assets;
using Domain.Inspections;
using Domain.WorkOrders.Enums;
using SharedKernel;

namespace Domain.WorkOrders;

public sealed class WorkOrder : Entity
{
    public Guid Id { get; set; }
    public string WorkOrderNumber { get; set; }
    public Guid VehicleId { get; set; }
    public Guid? VehicleInspectionId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public WorkOrderPriority Priority { get; set; }
    public WorkOrderStatus Status { get; set; }
    public Guid? AssignedTechnicianId { get; set; }
    public decimal? EstimatedCostQAR { get; set; }
    public decimal? ActualCostQAR { get; set; }
    public DateTime? ScheduledDate { get; set; }
    public DateTime? StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public int? JobType { get; set; }
    public string? DriverName { get; set; }
    public string? PreparedBy { get; set; }
    public string? CheckedByDriver { get; set; }
    public string? CheckedByMechanic { get; set; }
    public string? AuthorizedBy { get; set; }

    public Vehicle Vehicle { get; set; }
    public VehicleInspection? VehicleInspection { get; set; }

    public ICollection<WorkOrderItem> WorkOrderItems { get; set; } = [];
    public ICollection<WorkOrderStatusHistory> StatusHistory { get; set; } = [];
}
