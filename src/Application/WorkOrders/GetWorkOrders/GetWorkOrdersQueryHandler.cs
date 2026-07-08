using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.WorkOrders.GetWorkOrders;

internal sealed class GetWorkOrdersQueryHandler(IApplicationDbContext dbContext)
    : IQueryHandler<GetWorkOrdersQuery, List<WorkOrderResponse>>
{
    public async Task<Result<List<WorkOrderResponse>>> Handle(GetWorkOrdersQuery request, CancellationToken cancellationToken)
    {
        var entities = await dbContext.WorkOrders
            .AsNoTracking()
            .Include(wo => wo.Vehicle)
            .Include(wo => wo.WorkOrderItems)
            .Include(wo => wo.StatusHistory)
            .OrderByDescending(wo => wo.CreatedAt)
            .ToListAsync(cancellationToken);

        List<WorkOrderResponse> workOrders = entities.Select(wo => new WorkOrderResponse(
            wo.Id,
            wo.WorkOrderNumber,
            wo.VehicleId,
            wo.Vehicle.ChassisNumber,
            wo.VehicleInspectionId,
            wo.Title,
            wo.Description,
            wo.Priority,
            wo.Status,
            wo.AssignedTechnicianId,
            wo.EstimatedCostQAR,
            wo.ActualCostQAR,
            wo.ScheduledDate,
            wo.StartedAt,
            wo.CompletedAt,
            wo.JobType,
            wo.DriverName,
            wo.PreparedBy,
            wo.CheckedByDriver,
            wo.CheckedByMechanic,
            wo.AuthorizedBy,
            wo.WorkOrderItems.Select(i => new WorkOrderItemResponse(
                i.Id,
                i.ItemType,
                i.Description,
                i.Quantity,
                i.UnitCostQAR,
                i.TotalCostQAR,
                i.StartTime,
                i.EndTime,
                i.MechanicName,
                i.Remarks)).ToList(),
            wo.StatusHistory.Select(h => new WorkOrderStatusHistoryResponse(
                h.Id,
                h.PreviousStatus,
                h.NewStatus,
                h.ChangedByUserId,
                h.Notes,
                h.ChangedAt)).OrderByDescending(h => h.ChangedAt).ToList()))
            .ToList();

        return workOrders;
    }
}
