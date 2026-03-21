using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.WorkOrders;
using Domain.WorkOrders;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.WorkOrders.UpdateWorkOrder;

internal sealed class UpdateWorkOrderCommandHandler(
    IApplicationDbContext dbContext,
    IDateTimeProvider dateTimeProvider)
    : ICommandHandler<UpdateWorkOrderCommand>
{
    public async Task<Result> Handle(UpdateWorkOrderCommand request, CancellationToken cancellationToken)
    {
        var workOrder = await dbContext.WorkOrders
            .Include(w => w.WorkOrderItems)
            .FirstOrDefaultAsync(w => w.Id == request.Id, cancellationToken);

        if (workOrder is null)
        {
            return Result.Failure(WorkOrderErrors.NotFound(request.Id));
        }

        workOrder.Title = request.Title;
        workOrder.Description = request.Description;
        workOrder.Priority = request.Priority;
        workOrder.AssignedTechnicianId = request.AssignedTechnicianId;
        workOrder.EstimatedCostQAR = request.EstimatedCostQAR;
        workOrder.ScheduledDate = request.ScheduledDate;
        workOrder.UpdatedAt = dateTimeProvider.UtcNow;

        // Handle items
        var existingItems = workOrder.WorkOrderItems.ToList();
        var incomingItemIds = request.Items.Where(i => i.Id.HasValue).Select(i => i.Id!.Value).ToList();

        // Remove items
        foreach (var existingItem in existingItems)
        {
            if (!incomingItemIds.Contains(existingItem.Id))
            {
                dbContext.WorkOrderItems.Remove(existingItem);
            }
        }

        // Update or add items
        foreach (var itemCommand in request.Items)
        {
            if (itemCommand.Id.HasValue)
            {
                var existingItem = existingItems.FirstOrDefault(i => i.Id == itemCommand.Id.Value);
                if (existingItem != null)
                {
                    existingItem.ItemType = itemCommand.ItemType;
                    existingItem.Description = itemCommand.Description;
                    existingItem.Quantity = itemCommand.Quantity;
                    existingItem.UnitCostQAR = itemCommand.UnitCostQAR;
                    existingItem.TotalCostQAR = itemCommand.Quantity * itemCommand.UnitCostQAR;
                }
            }
            else
            {
                var newItem = new WorkOrderItem
                {
                    Id = Guid.NewGuid(),
                    WorkOrderId = workOrder.Id,
                    ItemType = itemCommand.ItemType,
                    Description = itemCommand.Description,
                    Quantity = itemCommand.Quantity,
                    UnitCostQAR = itemCommand.UnitCostQAR,
                    TotalCostQAR = itemCommand.Quantity * itemCommand.UnitCostQAR
                };
                dbContext.WorkOrderItems.Add(newItem);
            }
        }

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
