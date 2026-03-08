using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Drivers;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Drivers.Notifications.SendNotification;

internal sealed class SendNotificationCommandHandler(
    IApplicationDbContext dbContext,
    IDateTimeProvider dateTimeProvider) : ICommandHandler<SendNotificationCommand, Guid>
{
    public async Task<Result<Guid>> Handle(SendNotificationCommand request, CancellationToken cancellationToken)
    {
        bool driverExists = await dbContext.Drivers.AnyAsync(d => d.Id == request.DriverId, cancellationToken);
        if (!driverExists)
        {
            return Result.Failure<Guid>(DriverErrors.NotFound(request.DriverId));
        }

        var notification = new DriverNotification
        {
            Id = Guid.NewGuid(),
            DriverId = request.DriverId,
            NotificationType = request.NotificationType,
            Channel = request.Channel,
            Title = request.Title,
            Body = request.MessageBody,
            RelatedEntityId = request.RelatedEntityId,
            RelatedEntityType = request.MetaDataJson,
            SentAt = dateTimeProvider.UtcNow,
            IsRead = false
        };

        dbContext.DriverNotifications.Add(notification);
        await dbContext.SaveChangesAsync(cancellationToken);

        return notification.Id;
    }
}
