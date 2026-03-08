using Application.Abstractions.Messaging;
using Application.Fuel.Woqood.Queries.GetWoqoodImportBatch;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.WoqoodImport;

internal sealed class GetBatch : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("fuel/woqood/import/{batchId:guid}", async (
            Guid batchId,
            IQueryHandler<GetWoqoodImportBatchQuery, WoqoodImportBatchResponse> handler,
            CancellationToken cancellationToken) =>
        {
            var query = new GetWoqoodImportBatchQuery(batchId);

            Result<WoqoodImportBatchResponse> result = await handler.Handle(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.WoqoodImport);
    }
}
