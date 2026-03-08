using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Fuel;
using Domain.Fuel.Enums;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Fuel.Woqood.Commands.AllocateWoqoodTransaction;

internal sealed class AllocateWoqoodTransactionCommandHandler(
    IApplicationDbContext dbContext)
    : ICommandHandler<AllocateWoqoodTransactionCommand>
{
    public async Task<Result> Handle(AllocateWoqoodTransactionCommand request, CancellationToken cancellationToken)
    {
        Domain.Fuel.WoqoodFuelTransaction? transaction = await dbContext.WoqoodFuelTransactions
            .FirstOrDefaultAsync(t => t.Id == request.TransactionId, cancellationToken);

        if (transaction is null)
        {
            return Result.Failure(Error.NotFound("WoqoodFuelTransaction.NotFound", "Transaction not found."));
        }

        if (transaction.IsAllocated)
        {
            return Result.Failure(Error.Conflict("WoqoodFuelTransaction.AlreadyAllocated", "This transaction has already been allocated."));
        }

        var allocation = new FuelCostAllocation
        {
            Id = Guid.NewGuid(),
            WoqoodFuelTransactionId = transaction.Id,
            VehicleId = request.VehicleId,
            TripId = request.TripId,
            AllocationSource = FuelAllocationSource.WoqoodImport,
            QuantityLitres = transaction.QuantityLitres,
            AmountQAR = transaction.TotalAmountQAR,
            AllocationDate = DateOnly.FromDateTime(transaction.TransactionDate),
            AllocatedByUserId = request.AllocatedByUserId,
            Notes = request.Notes,
            CreatedAt = DateTime.UtcNow
        };

        transaction.IsAllocated = true;
        transaction.VehicleId = request.VehicleId;
        transaction.TripId = request.TripId;

        dbContext.FuelCostAllocations.Add(allocation);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
