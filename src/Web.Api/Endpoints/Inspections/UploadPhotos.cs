using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Application.Abstractions.Messaging;
using Application.Inspections.UploadInspectionPhotos;
using Domain.Inspections.Enums;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Inspections;

internal sealed class UploadPhotos : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("inspections/{id}/photos", async (
            Guid id,
            IFormFileCollection photos,
            [FromServices] ICommandHandler<UploadInspectionPhotosCommand> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new UploadInspectionPhotosCommand(
                id,
                photos.Select(f => new PhotoUploadItem(
                    f.OpenReadStream(), 
                    f.FileName, 
                    null, 
                    PhotoType.General)).ToList());

            Result result = await handler.Handle(command, cancellationToken);

            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .DisableAntiforgery() // If needed for file uploads in some environments
        .WithTags(Tags.Inspections);
    }
}
