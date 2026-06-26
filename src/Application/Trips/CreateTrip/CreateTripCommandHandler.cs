using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Trips;
using Domain.Trips.Enums;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Trips.CreateTrip;

internal sealed class CreateTripCommandHandler(
    IApplicationDbContext dbContext,
    IDateTimeProvider dateTimeProvider)
    : ICommandHandler<CreateTripCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateTripCommand request, CancellationToken cancellationToken)
    {
        if (request.TripCategoryMaterialId.HasValue)
        {
            var mappingExists = await dbContext.TripCategoryMaterials
                .AnyAsync(m => m.Id == request.TripCategoryMaterialId.Value && m.IsActive, cancellationToken);
            if (!mappingExists)
            {
                return Result.Failure<Guid>(Error.NotFound(
                    "TripCategoryMaterial.NotFound",
                    $"Trip category material mapping with ID '{request.TripCategoryMaterialId}' was not found."
                ));
            }

            if (!request.Quantity.HasValue || request.Quantity.Value <= 0)
            {
                return Result.Failure<Guid>(Error.Problem(
                    "Trip.QuantityRequired",
                    "Quantity is required and must be greater than 0 when Trip Category, Material, and UOM are selected."
                ));
            }
        }

        var trip = new Trip
        {
            Id = Guid.NewGuid(),
            TripNumber = request.TripNumber,
            DriverId = request.DriverId,
            VehicleId = request.VehicleId,
            TrailerId = request.TrailerId,
            ClientId = request.ClientId,
            Origin = request.Origin ?? string.Empty,
            Destination = request.Destination ?? string.Empty,
            Status = TripStatus.Scheduled,
            ScheduledStartAt = request.ScheduledStartAt,
            CreatedAt = dateTimeProvider.UtcNow,
            UpdatedAt = dateTimeProvider.UtcNow,
            IsImported = false,
            TripCategoryMaterialId = request.TripCategoryMaterialId,
            Quantity = request.Quantity
        };

        dbContext.Trips.Add(trip);

        await dbContext.SaveChangesAsync(cancellationToken);

        return trip.Id;
    }
}
