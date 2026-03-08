using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.WorkOrders.GetWorkOrders;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.WorkOrders.GetWorkOrderById;

internal sealed class GetWorkOrderByIdQueryHandler(IApplicationDbContext dbContext)
    : IQueryHandler<GetWorkOrderByIdQuery, WorkOrderResponse>
{
    public async Task<Result<WorkOrderResponse>> Handle(GetWorkOrderByIdQuery request, CancellationToken cancellationToken)
    {
        WorkOrderResponse? workOrder = await dbContext.WorkOrders
            .AsNoTracking()
            .Include(wo => wo.WorkOrderItems)
            .Include(wo => wo.StatusHistory)
            .Where(wo => wo.Id == request.WorkOrderId)
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
            .SingleOrDefaultAsync(cancellationToken);

        if (workOrder is null)
        {
            return Result.Failure<WorkOrderResponse>(WorkOrderErrors.NotFound(request.WorkOrderId));
        }

        return workOrder;
    }
}
