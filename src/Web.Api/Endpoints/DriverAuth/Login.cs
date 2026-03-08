using Application.Abstractions.Messaging;
using Application.Drivers.Auth.Login;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.DriverAuth;

internal sealed class Login : IEndpoint
{
    public sealed record Request(string Username, string Password, string? Platform = null, string? DeviceToken = null);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("drivers/auth/login", async (
            Request request,
            ICommandHandler<LoginCommand, LoginResponse> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new LoginCommand(request.Username, request.Password, request.Platform, request.DeviceToken);
            Result<LoginResponse> result = await handler.Handle(command, cancellationToken);
            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.DriverAuth);
    }
}
