using Application.Abstractions.Messaging;
using Application.Trips.GetImportBatches;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Trips;

public sealed class GetImportBatches : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("import-batches", async (
            IQueryHandler<GetImportBatchesQuery, List<Application.Trips.Common.ImportBatchResponse>> handler,
            CancellationToken cancellationToken) =>
        {
            var query = new GetImportBatchesQuery();

            Result<List<Application.Trips.Common.ImportBatchResponse>> result = await handler.Handle(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.Trips);
    }
}
