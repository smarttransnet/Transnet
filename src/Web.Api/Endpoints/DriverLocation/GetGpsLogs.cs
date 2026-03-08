using Application.Abstractions.Messaging;
using Application.Drivers.Location.GetGpsLogs;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.DriverLocation;

internal sealed class GetGpsLogsEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("drivers/{driverId:guid}/gps-logs", async (
            Guid driverId,
            [FromQuery] Guid? tripId,
            [FromQuery] int page,
            [FromQuery] int pageSize,
            IQueryHandler<GetGpsLogsQuery, PagedList<GpsLogResponse>> handler,
            CancellationToken cancellationToken) =>
        {
            var query = new GetGpsLogsQuery(driverId, tripId, page, pageSize);
            Result<PagedList<GpsLogResponse>> result = await handler.Handle(query, cancellationToken);
            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.DriverLocation);
    }
}
