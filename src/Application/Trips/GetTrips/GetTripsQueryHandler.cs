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
                null, // VehicleRegistrationNumber
                t.Client != null ? t.Client.CompanyName : null, // ClientName
                t.ClientId, // ClientId
                null,
                null,
                null,
                null,
                null,
                null, // VehiclePlateNumber
                null, // VehicleCategoryName
                t.SuptNo, // SuptNo
                t.SuptDocPath, // SuptDocPath
                t.TripCategoryMaterialId, // TripCategoryMaterialId
                t.TripCategoryMaterial != null && t.TripCategoryMaterial.TripCategory != null ? t.TripCategoryMaterial.TripCategory.CategoryName : null, // CategoryName
                t.TripCategoryMaterial != null && t.TripCategoryMaterial.Material != null ? t.TripCategoryMaterial.Material.MaterialName : null, // MaterialName
                t.TripCategoryMaterial != null && t.TripCategoryMaterial.Uom != null ? t.TripCategoryMaterial.Uom.UOMCode : null)) // UomCode
            .ToListAsync(cancellationToken);

        return trips;
    }
}
