using Application.Abstractions.Messaging;
using Domain.Drivers;
using Domain.Drivers.Enums;
namespace Application.Drivers.Location.RecordLocation; public sealed record RecordLocationCommand(Guid DriverId, Guid? TripId, double Latitude, double Longitude, float? Accuracy, float? SpeedKmh, float? Bearing, LocationSource Source) : ICommand<Guid>;
