using Application.Abstractions.Messaging;
using Application.Drivers.Attendance.CheckIn;
using Domain.Drivers.Enums;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.DriverAttendance;

internal sealed class CheckIn : IEndpoint
{
    public sealed record Request(double? Latitude, double? Longitude, string? Notes, AttendanceSource? Source);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("drivers/{driverId:guid}/attendance/check-in", async (
            Guid driverId,
            Request request,
            ICommandHandler<CheckInCommand, Guid> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new CheckInCommand(driverId, request.Latitude, request.Longitude, request.Notes, request.Source);
            Result<Guid> result = await handler.Handle(command, cancellationToken);
            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.DriverAttendance);
    }
}
