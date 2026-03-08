using Application.Abstractions.Messaging;
using Application.Inspections.GetVehicleInspections;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Inspections;

internal sealed class GetByVehicleId : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("inspections/vehicle/{vehicleId:guid}", async (
            Guid vehicleId,
            IQueryHandler<GetVehicleInspectionsQuery, List<VehicleInspectionResponse>> handler,
            CancellationToken cancellationToken) =>
        {
            var query = new GetVehicleInspectionsQuery(vehicleId);

            Result<List<VehicleInspectionResponse>> result = await handler.Handle(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.Inspections);
    }
}
