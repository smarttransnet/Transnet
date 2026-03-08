using Application.Abstractions.Messaging;
using Application.Fuel.Woqood.Queries.GetWoqoodBatchTransactions;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.WoqoodImport;

internal sealed class GetBatchTransactions : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("fuel/woqood/import/{batchId:guid}/transactions", async (
            Guid batchId,
            IQueryHandler<GetWoqoodBatchTransactionsQuery, IReadOnlyList<WoqoodFuelTransactionResponse>> handler,
            CancellationToken cancellationToken) =>
        {
            var query = new GetWoqoodBatchTransactionsQuery(batchId);

            Result<IReadOnlyList<WoqoodFuelTransactionResponse>> result = await handler.Handle(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.WoqoodImport);
    }
}
