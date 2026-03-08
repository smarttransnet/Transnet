using Application.Abstractions.Messaging;
using Application.Drivers;
using Application.Drivers.GetDriverById;
using Application.Drivers.GetDrivers;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Drivers;

internal sealed class GetById : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("drivers/{id:guid}", async (
            Guid id,
            IQueryHandler<GetDriverByIdQuery, DriverResponse> handler,
            CancellationToken cancellationToken) =>
        {
            Result<DriverResponse> result = await handler.Handle(new GetDriverByIdQuery(id), cancellationToken);
            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.Drivers);
    }
}
