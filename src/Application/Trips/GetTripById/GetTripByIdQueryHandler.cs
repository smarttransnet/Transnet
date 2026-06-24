using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Trips.Common;
using Domain.Trips;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Trips.GetTripById;

internal sealed class GetTripByIdQueryHandler : IQueryHandler<GetTripByIdQuery, TripResponse>
{
    private readonly IApplicationDbContext _context;

    public GetTripByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<TripResponse>> Handle(GetTripByIdQuery request, CancellationToken cancellationToken)
    {
        Trip? trip = await _context.Trips
            .Include(t => t.Stops)
            .Include(t => t.Halts)
            .Include(t => t.Voucher).ThenInclude(v => v!.CustomFields)
            .Include(t => t.PodUploads)
            .Include(t => t.TripCategoryMaterial).ThenInclude(m => m!.TripCategory)
            .Include(t => t.TripCategoryMaterial).ThenInclude(m => m!.Material)
            .Include(t => t.TripCategoryMaterial).ThenInclude(m => m!.Uom)
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

        if (trip is null)
        {
            return Result.Failure<TripResponse>(TripErrors.NotFound(request.Id));
        }

        string creatorName = "System Administrator";
        if (trip.Voucher != null)
        {
            creatorName = await _context.Users
                .Where(u => u.Id == trip.Voucher.CreatedByUserId)
                .Select(u => u.FirstName + " " + u.LastName)
                .FirstOrDefaultAsync(cancellationToken) ?? "System Administrator";
        }

        string driverName = await _context.Drivers
            .Where(d => d.Id == trip.DriverId)
            .Select(d => d.FirstName + " " + d.LastName)
            .FirstOrDefaultAsync(cancellationToken) ?? "Unknown Driver";

        var vehicle = await _context.Vehicles
            .Where(v => v.Id == trip.VehicleId)
            .Select(v => new { v.RegistrationNumber, v.PlateNumber, CategoryName = v.Category.Name })
            .FirstOrDefaultAsync(cancellationToken);

        string vehicleReg = vehicle?.RegistrationNumber ?? "Unknown Vehicle";
        string vehiclePlate = vehicle?.PlateNumber ?? "Unknown Plate";
        string vehicleCategory = vehicle?.CategoryName ?? "Unknown Category";

        string? clientName = null;
        if (trip.ClientId.HasValue)
        {
            clientName = await _context.Clients
                .Where(c => c.Id == trip.ClientId.Value)
                .Select(c => c.CompanyName)
                .FirstOrDefaultAsync(cancellationToken);
        }

        TripResponse response = new(
            Id: trip.Id,
            TripNumber: trip.TripNumber,
            DriverId: trip.DriverId,
            VehicleId: trip.VehicleId,
            TrailerId: trip.TrailerId,
            Status: trip.Status,
            ScheduledStartAt: trip.ScheduledStartAt,
            ActualStartAt: trip.ActualStartAt,
            ActualEndAt: trip.ActualEndAt,
            TotalDistanceKm: trip.TotalDistanceKm,
            IsImported: trip.IsImported,
            ImportBatchId: trip.ImportBatchId,
            Origin: trip.Origin,
            Destination: trip.Destination,
            DriverConfirmedAt: trip.DriverConfirmedAt,
            OfficeApprovedAt: trip.OfficeApprovedAt,
            OfficeApprovedByUserId: trip.OfficeApprovedByUserId,
            CreatedAt: trip.CreatedAt,
            UpdatedAt: trip.UpdatedAt,
            DriverName: driverName,
            VehicleRegistrationNumber: vehicleReg,
            VehiclePlateNumber: vehiclePlate,
            VehicleCategoryName: vehicleCategory,
            SuptNo: trip.SuptNo,
            SuptDocPath: trip.SuptDocPath,
            TripCategoryMaterialId: trip.TripCategoryMaterialId,
            CategoryName: trip.TripCategoryMaterial?.TripCategory?.CategoryName,
            MaterialName: trip.TripCategoryMaterial?.Material?.MaterialName,
            UomCode: trip.TripCategoryMaterial?.Uom?.UOMCode,
            ClientName: clientName,
            ClientId: trip.ClientId,
            ResponseVersion: "v2-with-names", // Diagnostic flag
            Stops: trip.Stops.OrderBy(s => s.StopOrder).Select(s => new TripStopResponse(
                s.Id,
                s.TripId,
                s.StopOrder,
                s.StopType,
                s.LocationName,
                s.Address,
                s.Latitude,
                s.Longitude,
                s.PocName,
                s.PocPhone,
                s.PocEmail,
                s.ScheduledArrivalAt,
                s.ActualArrivalAt,
                s.ActualDepartureAt,
                s.Notes)).ToList(),
            Halts: trip.Halts.OrderBy(h => h.StartedAt).Select(h => new TripHaltResponse(
                h.Id,
                h.TripId,
                h.HaltType,
                h.Reason,
                h.Latitude,
                h.Longitude,
                h.LocationName,
                h.StartedAt,
                h.EndedAt,
                h.DurationMinutes,
                h.RecordedByDriverId)).ToList(),
            Voucher: trip.Voucher is null ? null : new TripVoucherResponse(
                trip.Voucher.Id,
                trip.Voucher.TripId,
                trip.Voucher.VoucherNumber,
                trip.Voucher.VoucherDate,
                trip.Voucher.Notes,
                trip.Voucher.CreatedByUserId,
                creatorName,
                trip.Voucher.CreatedAt,
                trip.Voucher.CustomFields.Select(cf => new TripCustomFieldResponse(
                    cf.Id,
                    cf.TripId,
                    cf.TripVoucherId,
                    cf.FieldDefinitionId,
                    cf.Value)).ToList()),
            PodUploads: trip.PodUploads.OrderBy(p => p.UploadedAt).Select(p => new TripPodUploadResponse(
                p.Id,
                p.TripId,
                p.TripStopId,
                p.DocumentType,
                new Uri(p.FileUrl, UriKind.RelativeOrAbsolute),
                p.FileName,
                p.Latitude,
                p.Longitude,
                p.UploadedAt,
                p.UploadedByDriverId)).ToList());

        return Result.Success(response);
    }
}
