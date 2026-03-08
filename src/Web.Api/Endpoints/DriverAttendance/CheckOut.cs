using Application.Abstractions.Messaging;
using Application.Drivers.Attendance.CheckOut;
using Domain.Drivers.Enums;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.DriverAttendance;

internal sealed class CheckOut : IEndpoint
{
    public sealed record Request(double? Latitude, double? Longitude, string? Notes, AttendanceSource? Source);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("drivers/{driverId:guid}/attendance/check-out", async (
            Guid driverId,
            Request request,
            ICommandHandler<CheckOutCommand> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new CheckOutCommand(driverId, request.Latitude, request.Longitude, request.Notes, request.Source);
            Result result = await handler.Handle(command, cancellationToken);
            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithTags(Tags.DriverAttendance);
    }
}
