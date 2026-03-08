using Application.Abstractions.Messaging;
using Domain.Drivers;
namespace Application.Drivers.Location.GetLatestLocation; public sealed record GetLatestLocationQuery(Guid DriverId) : IQuery<LocationResponse>;
