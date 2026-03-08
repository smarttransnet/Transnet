using Application.Abstractions.Messaging;
using Application.VehicleCategories.GetVehicleCategories;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.VehicleCategories;

internal sealed class Get : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("vehicle-categories", async (
            IQueryHandler<GetVehicleCategoriesQuery, List<VehicleCategoryResponse>> handler,
            CancellationToken cancellationToken) =>
        {
            System.Diagnostics.Debugger.Break();
            var query = new GetVehicleCategoriesQuery();

            Result<List<VehicleCategoryResponse>> result = await handler.Handle(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.VehicleCategories);
    }
}
