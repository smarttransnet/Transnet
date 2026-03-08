using Application.Abstractions.Messaging;
using Application.Trips.ImportTrips;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Trips;

public sealed class ImportTrips : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("trips/import", async (
            IFormFile file, 
            Guid userId, 
            ICommandHandler<ImportTripsCommand, Guid> handler,
            CancellationToken cancellationToken) =>
        {
            if (file == null || file.Length == 0)
            {
                return Results.BadRequest("No file uploaded.");
            }

            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream, cancellationToken);

            var command = new ImportTripsCommand(
                file.FileName,
                memoryStream.ToArray(),
                userId);

            Result<Guid> result = await handler.Handle(command, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .DisableAntiforgery() // Simplified for development
        .WithTags(Tags.Trips);
    }
}
