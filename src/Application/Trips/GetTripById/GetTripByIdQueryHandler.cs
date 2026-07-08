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

            .Include(t => t.VehicleCategoryUom).ThenInclude(m => m!.VehicleCategory)
            .Include(t => t.VehicleCategoryUom).ThenInclude(m => m!.Uom)
            .Include(t => t.StatusHistory)
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

        if (trip is null || trip.Status == Domain.Trips.Enums.TripStatus.Deleted)
        {
            return Result.Failure<TripResponse>(TripErrors.NotFound(request.Id));
        }



        string driverName = await _context.Drivers
            .Where(d => d.Id == trip.DriverId)
            .Select(d => d.FirstName + " " + d.LastName)
            .FirstOrDefaultAsync(cancellationToken) ?? "Unknown Driver";

        var vehicle = await _context.Vehicles
            .Where(v => v.Id == trip.VehicleId)
            .Select(v => new { v.ChassisNumber, v.PlateNumber, CategoryName = v.Category.Name })
            .FirstOrDefaultAsync(cancellationToken);

        string vehicleChassis = vehicle?.ChassisNumber ?? "Unknown Vehicle";
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
            VehicleChassisNumber: vehicleChassis,
            VehiclePlateNumber: vehiclePlate,
            VehicleCategoryName: vehicleCategory,

            UomCode: trip.VehicleCategoryUom?.Uom?.UOMCode,
            VehicleCategoryUomId: trip.VehicleCategoryUomId,
            Quantity: trip.Quantity,
            ClientName: clientName,
            ClientId: trip.ClientId,
            ResponseVersion: null,

            StatusHistory: trip.StatusHistory.OrderBy(h => h.ChangedAt).Select(h => new TripStatusHistoryResponse(
                h.Id,
                h.TripId,
                h.PreviousStatus,
                h.NewStatus,
                h.ChangedByUserId,
                h.ChangedByDriverId,
                h.ChangedAt,
                h.Notes,
                h.Source)).ToList());

        return Result.Success(response);
    }
}
