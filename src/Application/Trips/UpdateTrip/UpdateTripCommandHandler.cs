using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Trips;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Trips.UpdateTrip;

internal sealed class UpdateTripCommandHandler : ICommandHandler<UpdateTripCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateTripCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(UpdateTripCommand request, CancellationToken cancellationToken)
    {
        Trip? trip = await _context.Trips
            .FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

        if (trip is null)
        {
            return Result.Failure(TripErrors.NotFound(request.Id));
        }

        if (request.VehicleCategoryUomId.HasValue)
        {
            var mappingExists = await _context.VehicleCategoryUoms
                .AnyAsync(m => m.Id == request.VehicleCategoryUomId.Value && m.IsActive, cancellationToken);
            if (!mappingExists)
            {
                return Result.Failure(Error.NotFound(
                    "VehicleCategoryUom.NotFound",
                    $"Vehicle category UOM mapping with ID '{request.VehicleCategoryUomId}' was not found."
                ));
            }

            if (!request.Quantity.HasValue || request.Quantity.Value <= 0)
            {
                return Result.Failure(Error.Problem(
                    "Trip.QuantityRequired",
                    "Quantity is required and must be greater than 0 when Vehicle Category and UOM are selected."
                ));
            }
        }

        trip.DriverId = request.DriverId;
        trip.VehicleId = request.VehicleId;
        trip.TrailerId = request.TrailerId;
        trip.ClientId = request.ClientId;
        trip.Origin = request.Origin ?? string.Empty;
        trip.Destination = request.Destination ?? string.Empty;
        trip.ScheduledStartAt = DateTime.SpecifyKind(request.ScheduledStartAt, DateTimeKind.Utc);
        trip.TotalDistanceKm = request.TotalDistanceKm;
        trip.VehicleCategoryUomId = request.VehicleCategoryUomId;
        trip.Quantity = request.Quantity;
        trip.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
