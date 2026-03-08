using Application.Abstractions.Messaging;
using Application.Drivers.Notifications.SendNotification;
using Domain.Drivers.Enums;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.DriverNotifications;

internal sealed class Send : IEndpoint
{
    public sealed record Request(NotificationType NotificationType, NotificationChannel Channel, string Title, string MessageBody, Guid? RelatedEntityId, string? MetaDataJson);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("drivers/{driverId:guid}/notifications", async (
            Guid driverId,
            Request request,
            ICommandHandler<SendNotificationCommand, Guid> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new SendNotificationCommand(
                driverId,
                request.NotificationType,
                request.Channel,
                request.Title,
                request.MessageBody,
                request.RelatedEntityId,
                request.MetaDataJson);

            Result<Guid> result = await handler.Handle(command, cancellationToken);
            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.DriverNotifications);
    }
}
