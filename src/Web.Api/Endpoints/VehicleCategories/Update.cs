using Application.Abstractions.Messaging;
using Application.VehicleCategories.UpdateVehicleCategory;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.VehicleCategories;

internal sealed class Update : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("vehicle-categories/{id}", async (
            Guid id,
            UpdateVehicleCategoryRequest request,
            ICommandHandler<UpdateVehicleCategoryCommand> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new UpdateVehicleCategoryCommand(id, request.Name, request.Description, request.IsActive);
            Result result = await handler.Handle(command, cancellationToken);

            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithTags(Tags.VehicleCategories);
    }
}

public sealed record UpdateVehicleCategoryRequest(string Name, string? Description, bool IsActive);
