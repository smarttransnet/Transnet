using Application.Abstractions.Messaging;
using Domain.Drivers;
using Domain.Drivers.Enums;
namespace Application.Drivers.Location.GetLatestLocation; public sealed record LocationResponse(Guid Id, Guid DriverId, Guid? TripId, double Latitude, double Longitude, float? Accuracy, float? SpeedKmh, float? Bearing, DateTime RecordedAt, LocationSource Source);
