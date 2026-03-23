using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.WorkOrders;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.WorkOrders.UpdateWorkOrder;

internal sealed class UpdateWorkOrderCommandHandler(IApplicationDbContext dbContext, IDateTimeProvider dateTimeProvider)
    : ICommandHandler<UpdateWorkOrderCommand>
{
    public async Task<Result> Handle(UpdateWorkOrderCommand request, CancellationToken cancellationToken)
    {
        WorkOrder? workOrder = await dbContext.WorkOrders
            .FirstOrDefaultAsync(w => w.Id == request.Id, cancellationToken);

        if (workOrder is null)
        {
            return Result.Failure(Error.NotFound("WorkOrder.NotFound", "The work order was not found."));
        }

        workOrder.Title = request.Title;
        workOrder.Description = request.Description;
        workOrder.Priority = request.Priority;
        workOrder.AssignedTechnicianId = request.AssignedTechnicianId;
        workOrder.EstimatedCostQAR = request.EstimatedCostQAR;
        workOrder.ScheduledDate = request.ScheduledDate;
        workOrder.UpdatedAt = dateTimeProvider.UtcNow;

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
