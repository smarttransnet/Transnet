using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Fuel.Woqood.Queries.GetWoqoodBatchTransactions;

internal sealed class GetWoqoodBatchTransactionsQueryHandler(
    IApplicationDbContext dbContext)
    : IQueryHandler<GetWoqoodBatchTransactionsQuery, IReadOnlyList<WoqoodFuelTransactionResponse>>
{
    public async Task<Result<IReadOnlyList<WoqoodFuelTransactionResponse>>> Handle(GetWoqoodBatchTransactionsQuery request, CancellationToken cancellationToken)
    {
        List<WoqoodFuelTransactionResponse> transactions = await dbContext.WoqoodFuelTransactions
            .Where(t => t.WoqoodImportBatchId == request.BatchId)
            .OrderBy(t => t.TransactionDate)
            .Select(t => new WoqoodFuelTransactionResponse(
                t.Id,
                t.WoqoodCardNumber,
                t.VehicleId,
                t.DriverId,
                t.TripId,
                t.TransactionDate,
                t.StationName,
                t.FuelType.ToString(),
                t.QuantityLitres,
                t.UnitPriceQAR,
                t.TotalAmountQAR,
                t.Odometer,
                t.IsAllocated
            ))
            .ToListAsync(cancellationToken);

        return transactions;
    }
}
