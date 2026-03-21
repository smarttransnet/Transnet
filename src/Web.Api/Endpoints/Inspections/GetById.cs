using Application.Abstractions.Messaging;
using Application.Inspections;
using Application.Inspections.GetVehicleInspectionById;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Inspections;

internal sealed class GetById : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("inspections/{id:guid}", async (
            Guid id,
            IQueryHandler<GetVehicleInspectionByIdQuery, VehicleInspectionResponse> handler,
            CancellationToken cancellationToken) =>
        {
            var query = new GetVehicleInspectionByIdQuery(id);

            Result<VehicleInspectionResponse> result = await handler.Handle(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.Inspections);
    }
}
