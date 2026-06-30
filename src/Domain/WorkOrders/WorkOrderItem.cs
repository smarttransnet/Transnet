using Domain.WorkOrders.Enums;
using SharedKernel;

namespace Domain.WorkOrders;

public sealed class WorkOrderItem : Entity
{
    public Guid Id { get; set; }
    public Guid WorkOrderId { get; set; }
    public WorkOrderItemType ItemType { get; set; }
    public string Description { get; set; }
    public decimal Quantity { get; set; }
    public decimal UnitCostQAR { get; set; }
    public decimal TotalCostQAR { get; set; }

    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public string? MechanicName { get; set; }
    public string? Remarks { get; set; }

    public WorkOrder WorkOrder { get; set; }
}
