using Application.Abstractions.Messaging;
using Application.VehicleCategories;
using Application.VehicleCategories.GetVehicleCategoryById;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.VehicleCategories;

internal sealed class GetById : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("vehicle-categories/{id:guid}", async (
            Guid id,
            IQueryHandler<GetVehicleCategoryByIdQuery, VehicleCategoryResponse> handler,
            CancellationToken cancellationToken) =>
        {
            var query = new GetVehicleCategoryByIdQuery(id);

            Result<VehicleCategoryResponse> result = await handler.Handle(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.VehicleCategories);
    }
}
