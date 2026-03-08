using Application.Abstractions.Messaging;
using Application.Drivers.Notifications.MarkNotificationRead;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.DriverNotifications;

internal sealed class MarkRead : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("drivers/notifications/{id:guid}/read", async (
            Guid id,
            ICommandHandler<MarkNotificationReadCommand> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new MarkNotificationReadCommand(id);
            Result result = await handler.Handle(command, cancellationToken);
            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithTags(Tags.DriverNotifications);
    }
}
