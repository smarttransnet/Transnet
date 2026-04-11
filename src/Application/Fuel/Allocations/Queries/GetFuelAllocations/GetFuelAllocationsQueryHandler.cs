using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Fuel.Allocations.Queries.GetFuelAllocations;

internal sealed class GetFuelAllocationsQueryHandler(
    IApplicationDbContext dbContext)
    : IQueryHandler<GetFuelAllocationsQuery, IReadOnlyList<FuelAllocationResponse>>
{
    public async Task<Result<IReadOnlyList<FuelAllocationResponse>>> Handle(GetFuelAllocationsQuery request, CancellationToken cancellationToken)
    {
        IQueryable<Domain.Fuel.FuelCostAllocation> query = dbContext.FuelCostAllocations.AsQueryable();

        if (request.VehicleId.HasValue)
        {
            query = query.Where(a => a.VehicleId == request.VehicleId.Value);
        }
            
        if (request.TripId.HasValue)
        {
            query = query.Where(a => a.TripId == request.TripId.Value);
        }

        if (request.StartDate.HasValue)
        {
            query = query.Where(a => a.AllocationDate >= request.StartDate.Value);
        }

        if (request.EndDate.HasValue)
        {
            query = query.Where(a => a.AllocationDate <= request.EndDate.Value);
        }

        List<FuelAllocationResponse> allocations = await query
            .Include(a => a.Vehicle)
            .OrderByDescending(a => a.AllocationDate)
            .Select(a => new FuelAllocationResponse(
                a.Id,
                a.WoqoodFuelTransactionId,
                a.DriverExpenseId,
                a.VehicleId,
                a.Vehicle != null ? a.Vehicle.PlateNumber : null,
                a.TripId,
                a.AllocationSource.ToString(),
                a.QuantityLitres,
                a.AmountQAR,
                a.FuelType.ToString(),
                a.AllocationDate,
                a.Notes
            ))
            .ToListAsync(cancellationToken);

        return allocations;
    }
}
