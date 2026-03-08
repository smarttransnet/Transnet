using Application.Abstractions.Messaging;
using Application.Trips.GetTripVoucher;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Trips;

public sealed class GetTripVoucher : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("trips/{id:guid}/voucher", async (
            Guid id, 
            IQueryHandler<GetTripVoucherQuery, Application.Trips.Common.TripVoucherResponse> handler,
            CancellationToken cancellationToken) =>
        {
            var query = new GetTripVoucherQuery(id);

            Result<Application.Trips.Common.TripVoucherResponse> result = await handler.Handle(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.Trips);
    }
}
