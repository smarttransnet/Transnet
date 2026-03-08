using Application.Abstractions.Messaging;
using Application.Drivers.Location.GetGpsLogs;
using Domain.Drivers;
namespace Application.Drivers.Location.GetGpsLogById; public sealed record GetGpsLogByIdQuery(Guid GpsLogId) : IQuery<GpsLogResponse>;
