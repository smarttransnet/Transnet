using Application.Abstractions.Messaging;
using Application.Drivers.Auth.CreateCredentials;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.DriverAuth;

internal sealed class CreateCredentials : IEndpoint
{
    public sealed record Request(string Password, Domain.Drivers.Enums.MobilePlatform Platform, string? DeviceToken = null);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("drivers/{driverId:guid}/credentials", async (
            Guid driverId,
            Request request,
            ICommandHandler<CreateCredentialsCommand, Guid> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new CreateCredentialsCommand(driverId, request.Password, request.Platform, request.DeviceToken);
            Result<Guid> result = await handler.Handle(command, cancellationToken);
            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.DriverAuth);
    }
}
