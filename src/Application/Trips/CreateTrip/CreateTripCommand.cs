using Application.Abstractions.Messaging;
using Domain.Trips.Enums;

namespace Application.Trips.CreateTrip;

public sealed record CreateTripCommand(
    string TripNumber,
    Guid DriverId,
    Guid VehicleId,
    Guid? TrailerId,
    DateTime ScheduledStartAt,
    Guid? ClientId,
    string Origin,
    string Destination,
    string? SuptNo = null,
    string? SuptDocPath = null,
    Guid? TripCategoryMaterialId = null) : ICommand<Guid>;
