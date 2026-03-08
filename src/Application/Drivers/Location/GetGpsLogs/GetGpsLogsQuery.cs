using Application.Abstractions.Messaging;
using Domain.Drivers;
using SharedKernel;
namespace Application.Drivers.Location.GetGpsLogs; public sealed record GetGpsLogsQuery(Guid? DriverId = null, Guid? TripId = null, int Page = 1, int PageSize = 10) : IQuery<PagedList<GpsLogResponse>>;
