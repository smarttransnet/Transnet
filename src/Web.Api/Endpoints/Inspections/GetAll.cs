using Application.Abstractions.Messaging;
using Application.Inspections;
using Application.Inspections.GetAllVehicleInspections;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Inspections;

internal sealed class GetAll : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("inspections", async (
            IQueryHandler<GetAllVehicleInspectionsQuery, List<VehicleInspectionResponse>> handler,
            CancellationToken cancellationToken) =>
        {
            var query = new GetAllVehicleInspectionsQuery();

            Result<List<VehicleInspectionResponse>> result = await handler.Handle(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.Inspections);
    }
}
