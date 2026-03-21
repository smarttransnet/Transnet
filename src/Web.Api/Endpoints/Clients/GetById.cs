using Application.Abstractions.Messaging;
using Application.Clients.Queries.GetClientById;
using Application.Clients.Queries.GetClients;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Clients;

internal sealed class GetById : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("clients/{id:guid}", async (
            Guid id,
            IQueryHandler<GetClientByIdQuery, ClientResponse> handler,
            CancellationToken cancellationToken) =>
        {
            var query = new GetClientByIdQuery(id);

            Result<ClientResponse> result = await handler.Handle(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.Clients);
    }
}
