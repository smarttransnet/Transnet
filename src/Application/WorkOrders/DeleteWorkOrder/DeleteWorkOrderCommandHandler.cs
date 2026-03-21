using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.WorkOrders;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.WorkOrders.DeleteWorkOrder;

internal sealed class DeleteWorkOrderCommandHandler(IApplicationDbContext context)
    : ICommandHandler<DeleteWorkOrderCommand>
{
    public async Task<Result> Handle(DeleteWorkOrderCommand command, CancellationToken cancellationToken)
    {
        WorkOrder? workOrder = await context.WorkOrders
            .Include(o => o.WorkOrderItems)
            .Include(o => o.StatusHistory)
            .FirstOrDefaultAsync(o => o.Id == command.Id, cancellationToken);

        if (workOrder is null)
        {
            return Result.Failure(Error.NotFound("WorkOrders.NotFound", $"The work order with the Id = '{command.Id}' was not found"));
        }

        context.WorkOrders.Remove(workOrder);

        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
