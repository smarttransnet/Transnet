using Application.Abstractions.Messaging;
using Domain.Drivers;
using Domain.Drivers.Enums;
using SharedKernel;
namespace Application.Drivers.Notifications.GetNotifications; public sealed record GetNotificationsQuery(Guid DriverId, bool? IsRead = null, int Page = 1, int PageSize = 20) : IQuery<PagedList<NotificationResponse>>;
