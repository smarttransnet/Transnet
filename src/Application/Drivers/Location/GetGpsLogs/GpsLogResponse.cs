using Application.Abstractions.Messaging;
using Domain.Drivers;
namespace Application.Drivers.Location.GetGpsLogs; public sealed record GpsLogResponse(Guid Id, Guid DriverId, Guid? TripId, DateTime SessionStart, DateTime? SessionEnd, decimal? TotalDistanceKm, float? MaxSpeedKmh, int PointCount, Uri? RawTrackUrl);
