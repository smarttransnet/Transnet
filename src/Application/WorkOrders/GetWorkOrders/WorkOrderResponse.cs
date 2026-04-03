using Domain.WorkOrders.Enums;

namespace Application.WorkOrders.GetWorkOrders;

public sealed record WorkOrderItemResponse(
    Guid Id,
    WorkOrderItemType ItemType,
    string Description,
    decimal Quantity,
    decimal UnitCostQAR,
    decimal TotalCostQAR);

public sealed record WorkOrderStatusHistoryResponse(
    Guid Id,
    WorkOrderStatus PreviousStatus,
    WorkOrderStatus NewStatus,
    Guid ChangedByUserId,
    string? Notes,
    DateTime ChangedAt);

public sealed record WorkOrderResponse(
    Guid Id,
    string WorkOrderNumber,
    Guid VehicleId,
    string VehicleRegistration,
    Guid? VehicleInspectionId,
    string Title,
    string Description,
    WorkOrderPriority Priority,
    WorkOrderStatus Status,
    Guid? AssignedTechnicianId,
    decimal? EstimatedCostQAR,
    decimal? ActualCostQAR,
    DateTime? ScheduledDate,
    DateTime? StartedAt,
    DateTime? CompletedAt,
    List<WorkOrderItemResponse> Items,
    List<WorkOrderStatusHistoryResponse> History);
