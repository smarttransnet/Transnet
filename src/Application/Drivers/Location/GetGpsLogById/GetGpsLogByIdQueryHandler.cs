using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Drivers.Location.GetGpsLogs;
using Domain.Drivers;
using Microsoft.EntityFrameworkCore;
using SharedKernel;
namespace Application.Drivers.Location.GetGpsLogById; internal sealed class GetGpsLogByIdQueryHandler(IApplicationDbContext dbContext) : IQueryHandler<GetGpsLogByIdQuery, GpsLogResponse> { public async Task<Result<GpsLogResponse>> Handle(GetGpsLogByIdQuery request, CancellationToken cancellationToken) { GpsLogResponse? log = await dbContext.DriverGpsLogs.AsNoTracking().Where(g => g.Id == request.GpsLogId).Select(g => new GpsLogResponse(g.Id, g.DriverId, g.TripId, g.SessionStart, g.SessionEnd, g.TotalDistanceKm, g.MaxSpeedKmh, g.PointCount, g.RawTrackUrl != null ? new Uri(g.RawTrackUrl) : null)).FirstOrDefaultAsync(cancellationToken); if (log is null) { return Result.Failure<GpsLogResponse>(Error.NotFound("GpsLog.NotFound", "The GPS log session was not found.")); } return log; } }
