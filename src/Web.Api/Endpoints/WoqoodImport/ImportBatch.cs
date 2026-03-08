using Application.Abstractions.Messaging;
using Application.Fuel.Woqood.Commands.ImportWoqoodBatch;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.WoqoodImport;

internal sealed class ImportBatch : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("fuel/woqood/import", async (
            IFormFile file,
            ICommandHandler<ImportWoqoodBatchCommand, Guid> handler,
            CancellationToken cancellationToken) =>
        {
            // Usually read file to byte[] or stream here
            // Mocking a file reading implementation to fit our scaffolded Application layer
            var mockUserId = Guid.NewGuid(); // To be inferred from Claims/User Context
            
            var command = new ImportWoqoodBatchCommand(
                mockUserId, 
                file.FileName, 
                Array.Empty<byte>()
            );

            Result<Guid> result = await handler.Handle(command, cancellationToken);
            
            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .DisableAntiforgery()
        .WithTags(Tags.WoqoodImport);
    }
}
