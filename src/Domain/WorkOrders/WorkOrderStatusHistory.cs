using Domain.WorkOrders.Enums;
using SharedKernel;

namespace Domain.WorkOrders;

public sealed class WorkOrderStatusHistory : Entity
{
    public Guid Id { get; set; }
    public Guid WorkOrderId { get; set; }
    public WorkOrderStatus PreviousStatus { get; set; }
    public WorkOrderStatus NewStatus { get; set; }
    public Guid ChangedByUserId { get; set; }
    public DateTime ChangedAt { get; set; }
    public string? Notes { get; set; }

    public WorkOrder WorkOrder { get; set; }
}
