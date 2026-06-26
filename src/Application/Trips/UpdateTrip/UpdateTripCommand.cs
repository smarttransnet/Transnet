using Application.Abstractions.Messaging;

namespace Application.Trips.UpdateTrip;

public sealed record UpdateTripCommand(
    Guid Id,
    Guid DriverId,
    Guid VehicleId,
    Guid? TrailerId,
    DateTime ScheduledStartAt,
    decimal? TotalDistanceKm,
    Guid? ClientId,
    string Origin,
    string Destination,
    Guid? TripCategoryMaterialId = null,
    decimal? Quantity = null) : ICommand;
