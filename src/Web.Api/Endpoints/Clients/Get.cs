using Application.Abstractions.Messaging;
using Application.Clients.Queries.GetClients;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Clients;

internal sealed class Get : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("clients", async (
            string? searchTerm,
            bool? isActive,
            IQueryHandler<GetClientsQuery, IReadOnlyList<ClientResponse>> handler,
            CancellationToken cancellationToken) =>
        {
            var query = new GetClientsQuery(searchTerm, isActive);

            Result<IReadOnlyList<ClientResponse>> result = await handler.Handle(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.Clients);
    }
}
