using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Fuel;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Fuel.Allocations.Commands.UpdateFuelAllocation;

internal sealed class UpdateFuelAllocationCommandHandler(
    IApplicationDbContext dbContext)
    : ICommandHandler<UpdateFuelAllocationCommand>
{
    public async Task<Result> Handle(UpdateFuelAllocationCommand request, CancellationToken cancellationToken)
    {
        FuelCostAllocation? allocation = await dbContext.FuelCostAllocations
            .FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);

        if (allocation is null)
        {
            return Result.Failure(Error.NotFound("FuelAllocation.NotFound", $"Fuel allocation with ID {request.Id} was not found."));
        }

        allocation.VehicleId = request.VehicleId;
        allocation.TripId = request.TripId;
        allocation.QuantityLitres = request.QuantityLitres;
        allocation.AmountQAR = request.AmountQAR;
        allocation.FuelType = request.FuelType;
        allocation.AllocationDate = request.AllocationDate;
        allocation.Notes = request.Notes;

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
