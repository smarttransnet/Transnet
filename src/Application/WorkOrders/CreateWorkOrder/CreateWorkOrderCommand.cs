using Application.Abstractions.Messaging;
using Domain.WorkOrders.Enums;

namespace Application.WorkOrders.CreateWorkOrder;

public sealed record CreateWorkOrderItemCommand(
    WorkOrderItemType ItemType,
    string Description,
    decimal Quantity,
    decimal UnitCostQAR,
    DateTime? StartTime,
    DateTime? EndTime,
    string? MechanicName,
    string? Remarks);

public sealed record CreateWorkOrderCommand(
    string Title,
    string Description,
    Guid VehicleId,
    Guid? VehicleInspectionId,
    WorkOrderPriority Priority,
    Guid? AssignedTechnicianId,
    decimal? EstimatedCostQAR,
    DateTime? ScheduledDate,
    int? JobType,
    string? DriverName,
    string? PreparedBy,
    string? CheckedByDriver,
    string? CheckedByMechanic,
    string? AuthorizedBy,
    DateTime? StartedAt,
    DateTime? CompletedAt,
    List<CreateWorkOrderItemCommand> Items) : ICommand<Guid>;
