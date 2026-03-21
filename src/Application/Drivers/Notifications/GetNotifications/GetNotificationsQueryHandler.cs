using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Drivers;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Drivers.Notifications.GetNotifications;

internal sealed class GetNotificationsQueryHandler(IApplicationDbContext dbContext)
    : IQueryHandler<GetNotificationsQuery, PagedList<NotificationResponse>>
{
    public async Task<Result<PagedList<NotificationResponse>>> Handle(GetNotificationsQuery request, CancellationToken cancellationToken)
    {
        IQueryable<DriverNotification> notificationsQuery = dbContext.DriverNotifications
            .Where(n => n.DriverId == request.DriverId);

        if (request.IsRead.HasValue)
        {
            notificationsQuery = notificationsQuery.Where(n => n.IsRead == request.IsRead.Value);
        }

        int totalCount = await notificationsQuery.CountAsync(cancellationToken);

        List<NotificationResponse> notifications = await notificationsQuery
            .OrderByDescending(n => n.SentAt)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(n => new NotificationResponse(
                n.Id,
                n.DriverId,
                n.NotificationType,
                n.Channel,
                n.Title,
                n.Body,
                n.RelatedEntityId,
                null, // MetaDataJson not in entity directly
                n.IsRead,
                n.SentAt,
                n.ReadAt))
            .ToListAsync(cancellationToken);

        return new PagedList<NotificationResponse>(notifications, totalCount, request.Page, request.PageSize);
    }
}
