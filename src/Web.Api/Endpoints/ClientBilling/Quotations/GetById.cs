using Application.Abstractions.Messaging;
using Application.Quotations.Queries.GetQuotationById;
using Application.Quotations.Queries.GetQuotations;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.ClientBilling.Quotations;

internal sealed class GetById : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("quotations/{id:guid}", async (
            Guid id,
            IQueryHandler<GetQuotationByIdQuery, QuotationDetailResponse> handler,
            CancellationToken cancellationToken) =>
        {
            var query = new GetQuotationByIdQuery(id);

            Result<QuotationDetailResponse> result = await handler.Handle(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.ClientBilling);
    }
}
