using Application.Abstractions.Messaging;
using Domain.Drivers;
namespace Application.Drivers.GetDriverById; public sealed record GetDriverByIdQuery(Guid DriverId) : IQuery<DriverResponse>;
