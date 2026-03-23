using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.WorkOrders;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.WorkOrders.DeleteWorkOrder;

internal sealed class DeleteWorkOrderCommandHandler(IApplicationDbContext dbContext)
    : ICommandHandler<DeleteWorkOrderCommand>
{
    public async Task<Result> Handle(DeleteWorkOrderCommand request, CancellationToken cancellationToken)
    {
        WorkOrder? workOrder = await dbContext.WorkOrders
            .FirstOrDefaultAsync(w => w.Id == request.Id, cancellationToken);

        if (workOrder is null)
        {
            return Result.Failure(Error.NotFound("WorkOrder.NotFound", "The work order was not found."));
        }

        dbContext.WorkOrders.Remove(workOrder);

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
