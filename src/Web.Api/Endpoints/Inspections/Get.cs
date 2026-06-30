using Microsoft.AspNetCore.Mvc;
using Application.Abstractions.Messaging;
using Microsoft.AspNetCore.Http;
using Application.Inspections.GetInspections;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Inspections;

internal sealed class Get : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("inspections", async (
            Guid? vehicleId,
            int? pageNumber,
            int? pageSize,
            [FromServices] IQueryHandler<GetInspectionsQuery, List<InspectionResponse>> handler,
            CancellationToken cancellationToken) =>
        {
            var query = new GetInspectionsQuery(vehicleId, pageNumber ?? 1, pageSize ?? 20);
            
            Result<List<InspectionResponse>> result = await handler.Handle(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.Inspections);
    }
}
