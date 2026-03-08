using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.WorkOrders;
using Domain.WorkOrders.Enums;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.WorkOrders.UpdateWorkOrderStatus;

internal sealed class UpdateWorkOrderStatusCommandHandler(
    IApplicationDbContext dbContext,
    IDateTimeProvider dateTimeProvider)
    : ICommandHandler<UpdateWorkOrderStatusCommand>
{
    public async Task<Result> Handle(UpdateWorkOrderStatusCommand request, CancellationToken cancellationToken)
    {
        WorkOrder? workOrder = await dbContext.WorkOrders
            .FirstOrDefaultAsync(w => w.Id == request.WorkOrderId, cancellationToken);

        if (workOrder is null)
        {
            return Result.Failure(WorkOrderErrors.NotFound(request.WorkOrderId));
        }

        if (workOrder.Status == request.Status)
        {
            return Result.Success(); // No change
        }

        DateTime now = dateTimeProvider.UtcNow;
        WorkOrderStatus previousStatus = workOrder.Status;
        workOrder.Status = request.Status;
        workOrder.UpdatedAt = now;

        if (request.Status == WorkOrderStatus.Completed || request.Status == WorkOrderStatus.Cancelled)
        {
            workOrder.CompletedAt = now;
        }

        var history = new WorkOrderStatusHistory
        {
            Id = Guid.NewGuid(),
            WorkOrderId = request.WorkOrderId,
            PreviousStatus = previousStatus,
            NewStatus = request.Status,
            Notes = request.Notes,
            ChangedByUserId = request.ChangedByUserId,
            ChangedAt = now
        };

        dbContext.WorkOrderStatusHistories.Add(history);

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
