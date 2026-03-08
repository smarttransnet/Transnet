using Application.Abstractions.Messaging;
using Application.Drivers.Auth.Refresh;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.DriverAuth;

internal sealed class Refresh : IEndpoint
{
    public sealed record Request(Guid DriverId, string RefreshToken);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("drivers/auth/refresh", async (
            Request request,
            ICommandHandler<RefreshCommand, RefreshResponse> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new RefreshCommand(request.DriverId, request.RefreshToken);
            Result<RefreshResponse> result = await handler.Handle(command, cancellationToken);
            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.DriverAuth);
    }
}
