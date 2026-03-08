using Application.Abstractions.Messaging;
using Application.VehicleCategories.CreateVehicleCategory;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.VehicleCategories;

internal sealed class Create : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("vehicle-categories", async (
            CreateVehicleCategoryCommand request,
            ICommandHandler<CreateVehicleCategoryCommand, Guid> handler,
            CancellationToken cancellationToken) =>
        {
            Result<Guid> result = await handler.Handle(request, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.VehicleCategories);
    }
}
