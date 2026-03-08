using Application.Abstractions.Messaging;
using Application.Drivers.Auth.Logout;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.DriverAuth;

internal sealed class Logout : IEndpoint
{
    public sealed record Request(Guid DriverId);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("drivers/auth/logout", async (
            Request request,
            ICommandHandler<LogoutCommand> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new LogoutCommand(request.DriverId);
            Result result = await handler.Handle(command, cancellationToken);
            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithTags(Tags.DriverAuth);
    }
}
