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
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

        if (trip is null)
        {
            return Result.Failure<TripResponse>(TripErrors.NotFound(request.Id));
        }

        TripResponse response = new(
            trip.Id,
            trip.TripNumber,
            trip.DriverId,
            trip.VehicleId,
            trip.TrailerId,
            trip.Status,
            trip.ScheduledStartAt,
            trip.ActualStartAt,
            trip.ActualEndAt,
            trip.TotalDistanceKm,
            trip.IsImported,
            trip.ImportBatchId,
            trip.Origin,
            trip.Destination,
            trip.DriverConfirmedAt,
            trip.OfficeApprovedAt,
            trip.OfficeApprovedByUserId,
            trip.CreatedAt,
            trip.UpdatedAt,
            trip.Stops.OrderBy(s => s.StopOrder).Select(s => new TripStopResponse(
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
            trip.Halts.OrderBy(h => h.StartedAt).Select(h => new TripHaltResponse(
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
            trip.Voucher is null ? null : new TripVoucherResponse(
                trip.Voucher.Id,
                trip.Voucher.TripId,
                trip.Voucher.VoucherNumber,
                trip.Voucher.VoucherDate,
                trip.Voucher.Notes,
                trip.Voucher.CreatedByUserId,
                trip.Voucher.CreatedAt,
                trip.Voucher.CustomFields.Select(cf => new TripCustomFieldResponse(
                    cf.Id,
                    cf.TripId,
                    cf.TripVoucherId,
                    cf.FieldDefinitionId,
                    cf.Value)).ToList()),
            trip.PodUploads.OrderBy(p => p.UploadedAt).Select(p => new TripPodUploadResponse(
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
