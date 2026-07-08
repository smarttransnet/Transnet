using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Trips.Common;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Trips.GetTrips;

internal sealed class GetTripsQueryHandler(IApplicationDbContext dbContext)
    : IQueryHandler<GetTripsQuery, List<TripResponse>>
{
    public async Task<Result<List<TripResponse>>> Handle(GetTripsQuery request, CancellationToken cancellationToken)
    {
        List<TripResponse> trips = await dbContext.Trips
            .AsNoTracking()
            .Where(t => t.Status != Domain.Trips.Enums.TripStatus.Deleted)
            .Select(t => new TripResponse(
                t.Id,
                t.TripNumber,
                t.DriverId,
                t.VehicleId,
                t.TrailerId,
                t.Status,
                t.ScheduledStartAt,
                t.ActualStartAt,
                t.ActualEndAt,
                t.TotalDistanceKm,
                t.IsImported,
                t.ImportBatchId,
                t.Origin,
                t.Destination,
                t.DriverConfirmedAt,
                t.OfficeApprovedAt,
                t.OfficeApprovedByUserId,
                t.CreatedAt,
                t.UpdatedAt,
                null, // DriverName
                null, // VehicleChassisNumber
                t.Client != null ? t.Client.CompanyName : null, // ClientName
                t.ClientId, // ClientId
                null,

                null, // VehiclePlateNumber
                t.VehicleCategoryUom != null && t.VehicleCategoryUom.VehicleCategory != null ? t.VehicleCategoryUom.VehicleCategory.Name : null, // VehicleCategoryName

                t.VehicleCategoryUom != null && t.VehicleCategoryUom.Uom != null ? t.VehicleCategoryUom.Uom.UOMCode : null, // UomCode
                t.VehicleCategoryUomId, // VehicleCategoryUomId
                t.Quantity)) // Quantity
            .ToListAsync(cancellationToken);

        return trips;
    }
}
