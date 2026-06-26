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

        if (request.TripCategoryMaterialId.HasValue)
        {
            var mappingExists = await _context.TripCategoryMaterials
                .AnyAsync(m => m.Id == request.TripCategoryMaterialId.Value && m.IsActive, cancellationToken);
            if (!mappingExists)
            {
                return Result.Failure(Error.NotFound(
                    "TripCategoryMaterial.NotFound",
                    $"Trip category material mapping with ID '{request.TripCategoryMaterialId}' was not found."
                ));
            }

            if (!request.Quantity.HasValue || request.Quantity.Value <= 0)
            {
                return Result.Failure(Error.Problem(
                    "Trip.QuantityRequired",
                    "Quantity is required and must be greater than 0 when Trip Category, Material, and UOM are selected."
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
        trip.TripCategoryMaterialId = request.TripCategoryMaterialId;
        trip.Quantity = request.Quantity;
        trip.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
