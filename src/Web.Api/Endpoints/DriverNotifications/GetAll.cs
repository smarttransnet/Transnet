using Application.Abstractions.Messaging;
using Application.Drivers.Notifications.GetNotifications;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.DriverNotifications;

internal sealed class GetAll : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("drivers/{driverId:guid}/notifications", async (
            Guid driverId,
            [FromQuery] bool unreadOnly,
            [FromQuery] int page,
            [FromQuery] int pageSize,
            IQueryHandler<GetNotificationsQuery, PagedList<NotificationResponse>> handler,
            CancellationToken cancellationToken) =>
        {
            var query = new GetNotificationsQuery(driverId, unreadOnly, page, pageSize);
            Result<PagedList<NotificationResponse>> result = await handler.Handle(query, cancellationToken);
            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.DriverNotifications);
    }
}
