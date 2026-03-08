using Application.Abstractions.Messaging;
using Domain.Drivers;
using Domain.Drivers.Enums;
namespace Application.Drivers.Notifications.GetNotifications; public sealed record NotificationResponse(Guid Id, Guid DriverId, NotificationType NotificationType, NotificationChannel Channel, string Title, string MessageBody, Guid? RelatedEntityId, string? MetaDataJson, bool IsRead, DateTime CreatedAt, DateTime? ReadAt);
