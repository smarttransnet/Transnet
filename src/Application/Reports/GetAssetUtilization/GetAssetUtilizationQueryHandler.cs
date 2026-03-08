using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Reports.GetAssetUtilization;

internal sealed class GetAssetUtilizationQueryHandler(IApplicationDbContext dbContext)
    : IQueryHandler<GetAssetUtilizationQuery, List<AssetUtilizationResponse>>
{
    public async Task<Result<List<AssetUtilizationResponse>>> Handle(GetAssetUtilizationQuery request, CancellationToken cancellationToken)
    {
        // A placeholder for actual complex report logic using telemetry, trips, etc.
        List<AssetUtilizationResponse> utilization = await dbContext.Vehicles
            .AsNoTracking()
            .Where(v => v.IsActive)
            .Select(v => new AssetUtilizationResponse(
                v.Id,
                v.RegistrationNumber,
                v.OdometerReading * 0.1m, // Example mock data
                5, // Example mock data for total trips
                12 // Example mock data for active days
            ))
            .ToListAsync(cancellationToken);

        return utilization;
    }
}
