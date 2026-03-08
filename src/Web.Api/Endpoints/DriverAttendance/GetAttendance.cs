using Application.Abstractions.Messaging;
using Application.Drivers.Attendance.GetAttendance;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.DriverAttendance;

internal sealed class GetAttendanceEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("drivers/{driverId:guid}/attendance", async (
            Guid driverId,
            [FromQuery] DateOnly? startDate,
            [FromQuery] DateOnly? endDate,
            [FromQuery] int page,
            [FromQuery] int pageSize,
            IQueryHandler<GetAttendanceQuery, PagedList<AttendanceResponse>> handler,
            CancellationToken cancellationToken) =>
        {
            var query = new GetAttendanceQuery(driverId, startDate, endDate, page, pageSize);
            Result<PagedList<AttendanceResponse>> result = await handler.Handle(query, cancellationToken);
            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.DriverAttendance);
    }
}
