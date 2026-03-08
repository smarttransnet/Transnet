
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Drivers;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Drivers.Notifications.MarkNotificationRead;

internal sealed class MarkNotificationReadCommandHandler(
    IApplicationDbContext dbContext,
    IDateTimeProvider dateTimeProvider) : ICommandHandler<MarkNotificationReadCommand>
{
    public async Task<Result> Handle(MarkNotificationReadCommand request, CancellationToken cancellationToken)
    {
        DriverNotification? notification = await dbContext.DriverNotifications
            .FirstOrDefaultAsync(n => n.Id == request.NotificationId, cancellationToken);

        if (notification is null)
        {
            return Result.Failure(Error.NotFound("Notifications.NotFound", "The notification was not found."));
        }

        if (!notification.IsRead)
        {
            notification.IsRead = true;
            notification.ReadAt = dateTimeProvider.UtcNow;
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        return Result.Success();
    }
}
