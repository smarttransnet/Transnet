using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Assets;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Assets.GetVehicles;

internal sealed class GetVehiclesQueryHandler(IApplicationDbContext dbContext)
    : IQueryHandler<GetVehiclesQuery, List<VehicleResponse>>
{
    public async Task<Result<List<VehicleResponse>>> Handle(GetVehiclesQuery request, CancellationToken cancellationToken)
    {
        List<VehicleResponse> vehicles = await dbContext.Vehicles
            .AsNoTracking()
            .Select(v => new VehicleResponse(
                v.Id,
                v.RegistrationNumber,
                v.PlateNumber,
                v.Make,
                v.Model,
                v.Year,
                v.VehicleCategoryId,
                v.VehicleType,
                v.Status,
                v.CurrentDriverId,
                v.CurrentLocationId,
                v.OdometerReading,
                v.IsActive,
                v.Category.Name))
            .ToListAsync(cancellationToken);

        return vehicles;
    }
}
