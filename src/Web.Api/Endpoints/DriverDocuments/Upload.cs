using Application.Abstractions.Messaging;
using Application.Drivers.Documents.UploadDocument;
using Domain.Drivers.Enums;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.DriverDocuments;

internal sealed class Upload : IEndpoint
{
    public sealed record Request(Guid? TripId, DriverDocumentType DocumentType, string Title, Uri FileUrl, bool SubmittedFromApp, double? Latitude, double? Longitude);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("drivers/{driverId:guid}/documents", async (
            Guid driverId,
            Request request,
            ICommandHandler<UploadDocumentCommand, Guid> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new UploadDocumentCommand(
                driverId,
                request.TripId,
                request.DocumentType,
                request.Title,
                request.FileUrl,
                request.SubmittedFromApp,
                request.Latitude,
                request.Longitude);

            Result<Guid> result = await handler.Handle(command, cancellationToken);
            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.DriverDocuments);
    }
}
