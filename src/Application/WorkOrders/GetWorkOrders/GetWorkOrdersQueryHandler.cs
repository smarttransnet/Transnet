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
        List<WorkOrderResponse> workOrders = await dbContext.WorkOrders
            .AsNoTracking()
            .Include(wo => wo.WorkOrderItems)
            .Include(wo => wo.StatusHistory)
            .OrderByDescending(wo => wo.CreatedAt)
            .Select(wo => new WorkOrderResponse(
                wo.Id,
                wo.WorkOrderNumber,
                wo.VehicleId,
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
                wo.WorkOrderItems.Select(i => new WorkOrderItemResponse(
                    i.Id,
                    i.ItemType,
                    i.Description,
                    i.Quantity,
                    i.UnitCostQAR,
                    i.TotalCostQAR)).ToList(),
                wo.StatusHistory.Select(h => new WorkOrderStatusHistoryResponse(
                    h.Id,
                    h.PreviousStatus,
                    h.NewStatus,
                    h.ChangedByUserId,
                    h.Notes,
                    h.ChangedAt)).OrderByDescending(h => h.ChangedAt).ToList()))
            .ToListAsync(cancellationToken);

        return workOrders;
    }
}
