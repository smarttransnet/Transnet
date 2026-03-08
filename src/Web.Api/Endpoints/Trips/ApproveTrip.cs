using Application.Abstractions.Messaging;
using Application.Trips.ApproveTrip;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Trips;

public sealed class ApproveTrip : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("trips/{id:guid}/approve", async (
            Guid id, 
            ApproveTripRequest request, 
            ICommandHandler<ApproveTripCommand> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new ApproveTripCommand(id, request.UserId);

            Result result = await handler.Handle(command, cancellationToken);

            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithTags(Tags.Trips);
    }
}

public sealed record ApproveTripRequest(Guid UserId);
