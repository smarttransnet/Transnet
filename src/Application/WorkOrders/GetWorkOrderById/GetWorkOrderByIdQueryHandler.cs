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
        var entity = await dbContext.WorkOrders
            .AsNoTracking()
            .Include(wo => wo.Vehicle)
            .Include(wo => wo.WorkOrderItems)
            .Include(wo => wo.StatusHistory)
            .FirstOrDefaultAsync(wo => wo.Id == request.WorkOrderId, cancellationToken);

        if (entity is null)
        {
            return Result.Failure<WorkOrderResponse>(WorkOrderErrors.NotFound(request.WorkOrderId));
        }

        WorkOrderResponse workOrder = new WorkOrderResponse(
            entity.Id,
            entity.WorkOrderNumber,
            entity.VehicleId,
            entity.Vehicle.RegistrationNumber,
            entity.VehicleInspectionId,
            entity.Title,
            entity.Description,
            entity.Priority,
            entity.Status,
            entity.AssignedTechnicianId,
            entity.EstimatedCostQAR,
            entity.ActualCostQAR,
            entity.ScheduledDate,
            entity.StartedAt,
            entity.CompletedAt,
            entity.WorkOrderItems.Select(i => new WorkOrderItemResponse(
                i.Id,
                i.ItemType,
                i.Description,
                i.Quantity,
                i.UnitCostQAR,
                i.TotalCostQAR)).ToList(),
            entity.StatusHistory.Select(h => new WorkOrderStatusHistoryResponse(
                h.Id,
                h.PreviousStatus,
                h.NewStatus,
                h.ChangedByUserId,
                h.Notes,
                h.ChangedAt)).OrderByDescending(h => h.ChangedAt).ToList());

        return workOrder;
    }
}
