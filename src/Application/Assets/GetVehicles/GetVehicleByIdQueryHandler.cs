using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Assets;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Assets.GetVehicles;

internal sealed class GetVehicleByIdQueryHandler(IApplicationDbContext dbContext)
    : IQueryHandler<GetVehicleByIdQuery, VehicleResponse>
{
    public async Task<Result<VehicleResponse>> Handle(GetVehicleByIdQuery request, CancellationToken cancellationToken)
    {
        VehicleResponse? vehicle = await dbContext.Vehicles
            .AsNoTracking()
            .Where(v => v.Id == request.VehicleId)
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
            .SingleOrDefaultAsync(cancellationToken);

        if (vehicle is null)
        {
            return Result.Failure<VehicleResponse>(VehicleErrors.NotFound(request.VehicleId));
        }

        return vehicle;
    }
}
