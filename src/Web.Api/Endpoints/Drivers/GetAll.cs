using Application.Abstractions.Messaging;
using Application.Drivers;
using Application.Drivers.GetDrivers;
using Domain.Drivers.Enums;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Drivers;

internal sealed class GetAll : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("drivers", async (
            [FromQuery] string? searchTerm,
            [FromQuery] DriverStatus? status,
            [FromQuery] bool? isActive,
            [FromQuery] int? page,
            [FromQuery] int? pageSize,
            IQueryHandler<GetDriversQuery, PagedList<DriverResponse>> handler,
            CancellationToken cancellationToken) =>
        {
            var query = new GetDriversQuery(
                searchTerm, 
                status, 
                isActive, 
                page ?? 1, 
                pageSize ?? 10);
            Result<PagedList<DriverResponse>> result = await handler.Handle(query, cancellationToken);
            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.Drivers);
    }
}
