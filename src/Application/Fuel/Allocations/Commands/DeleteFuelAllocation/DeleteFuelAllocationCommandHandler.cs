using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Fuel;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Fuel.Allocations.Commands.DeleteFuelAllocation;

internal sealed class DeleteFuelAllocationCommandHandler(
    IApplicationDbContext dbContext)
    : ICommandHandler<DeleteFuelAllocationCommand>
{
    public async Task<Result> Handle(DeleteFuelAllocationCommand request, CancellationToken cancellationToken)
    {
        FuelCostAllocation? allocation = await dbContext.FuelCostAllocations
            .FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);

        if (allocation is null)
        {
            return Result.Failure(Error.NotFound("FuelAllocation.NotFound", $"Fuel allocation with ID {request.Id} was not found."));
        }

        dbContext.FuelCostAllocations.Remove(allocation);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
