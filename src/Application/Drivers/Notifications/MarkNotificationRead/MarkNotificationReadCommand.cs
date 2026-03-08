using Application.Abstractions.Messaging;

namespace Application.Drivers.Notifications.MarkNotificationRead;

public sealed record MarkNotificationReadCommand(Guid NotificationId) : ICommand;
