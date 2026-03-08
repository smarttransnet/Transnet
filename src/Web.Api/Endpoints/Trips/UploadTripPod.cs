using Application.Abstractions.Messaging;
using Application.Trips.UploadTripPod;
using Domain.Trips.Enums;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Trips;

public sealed class UploadTripPod : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("trips/{id:guid}/pod-uploads", async (
            Guid id, 
            UploadTripPodRequest request, 
            ICommandHandler<UploadTripPodCommand, Guid> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new UploadTripPodCommand(
                id,
                request.TripStopId,
                request.DocumentType,
                request.FileUrl,
                request.FileName,
                request.Latitude,
                request.Longitude,
                request.UploadedByDriverId);

            Result<Guid> result = await handler.Handle(command, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.Trips);
    }
}

public sealed record UploadTripPodRequest(
    Guid? TripStopId,
    PodDocumentType DocumentType,
    Uri FileUrl,
    string FileName,
    double? Latitude,
    double? Longitude,
    Guid UploadedByDriverId);
