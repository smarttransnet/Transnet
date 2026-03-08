using Application.Abstractions.Messaging;
using Application.Drivers.Location.GetGpsLogById;
using Application.Drivers.Location.GetGpsLogs;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.DriverLocation;

internal sealed class GetGpsLogByIdEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("drivers/gps-logs/{id:guid}", async (
            Guid id,
            IQueryHandler<GetGpsLogByIdQuery, GpsLogResponse> handler,
            CancellationToken cancellationToken) =>
        {
            Result<GpsLogResponse> result = await handler.Handle(new GetGpsLogByIdQuery(id), cancellationToken);
            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.DriverLocation);
    }
}
