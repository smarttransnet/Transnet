using Application.Abstractions.Messaging;
using Application.Drivers.Assignments.GetAssignments;
using Domain.Drivers.Enums;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.DriverAssignments;

internal sealed class GetAll : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("drivers/{driverId:guid}/assignments", async (
            Guid driverId,
            [FromQuery] AssignmentStatus? status,
            [FromQuery] int? page,
            [FromQuery] int? pageSize,
            IQueryHandler<GetAssignmentsQuery, PagedList<AssignmentResponse>> handler,
            CancellationToken cancellationToken) =>
        {
            var query = new GetAssignmentsQuery(driverId, status, page ?? 1, pageSize ?? 10);
            Result<PagedList<AssignmentResponse>> result = await handler.Handle(query, cancellationToken);
            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.DriverAssignments);
    }
}
