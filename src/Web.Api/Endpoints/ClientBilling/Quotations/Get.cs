using Application.Abstractions.Messaging;
using Application.Quotations.Queries.GetQuotations;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.ClientBilling.Quotations;

internal sealed class Get : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("quotations", async (
            Guid? clientId,
            Domain.Billing.Enums.QuotationStatus? status,
            string? searchTerm,
            IQueryHandler<GetQuotationsQuery, IReadOnlyList<QuotationResponse>> handler,
            CancellationToken cancellationToken) =>
        {
            var query = new GetQuotationsQuery(clientId, status, searchTerm);

            Result<IReadOnlyList<QuotationResponse>> result = await handler.Handle(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.ClientBilling);
    }
}
