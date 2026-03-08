using Application.Abstractions.Messaging;
using Application.Trips.DeleteTripPod;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Trips;

public sealed class DeleteTripPod : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("trips/{id:guid}/pod-uploads/{docId:guid}", async (
            Guid id, 
            Guid docId, 
            ICommandHandler<DeleteTripPodCommand> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new DeleteTripPodCommand(docId, id);

            Result result = await handler.Handle(command, cancellationToken);

            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithTags(Tags.Trips);
    }
}
