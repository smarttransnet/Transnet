using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Fuel;
using Domain.Fuel.Enums;
using SharedKernel;

namespace Application.Fuel.Allocations.Commands.CreateFuelAllocation;

internal sealed class CreateFuelAllocationCommandHandler(
    IApplicationDbContext dbContext)
    : ICommandHandler<CreateFuelAllocationCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateFuelAllocationCommand request, CancellationToken cancellationToken)
    {
        var allocation = new FuelCostAllocation
        {
            Id = Guid.NewGuid(),
            VehicleId = request.VehicleId,
            TripId = request.TripId,
            AllocationSource = FuelAllocationSource.ManualEntry,
            QuantityLitres = request.QuantityLitres,
            AmountQAR = request.AmountQAR,
            FuelType = request.FuelType,
            AllocationDate = request.AllocationDate,
            AllocatedByUserId = request.UserId,
            Notes = request.Notes,
            CreatedAt = DateTime.UtcNow
        };

        dbContext.FuelCostAllocations.Add(allocation);
        await dbContext.SaveChangesAsync(cancellationToken);

        return allocation.Id;
    }
}
