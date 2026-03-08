using Application.Abstractions.Messaging;
using Domain.Drivers.Enums;

namespace Application.Drivers.Notifications.SendNotification;

public sealed record SendNotificationCommand(
    Guid DriverId,
    NotificationType NotificationType,
    NotificationChannel Channel,
    string Title,
    string MessageBody,
    Guid? RelatedEntityId,
    string? MetaDataJson) : ICommand<Guid>;
