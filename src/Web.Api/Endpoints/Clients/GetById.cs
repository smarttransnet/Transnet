using Application.Abstractions.Messaging;
using Application.Clients.Queries.GetClientById;
using Application.Clients.Queries.GetClients;
using Microsoft.AspNetCore.Http;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Clients;

internal sealed class GetById : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("clients/{id}", async (
            Guid id,
            IQueryHandler<GetClientByIdQuery, ClientResponse> handler,
            CancellationToken cancellationToken) =>
        {
            var query = new GetClientByIdQuery(id);
            Result<ClientResponse> result = await handler.Handle(query, cancellationToken);
            return result.IsSuccess ? Results.Ok(result.Value) : CustomResults.Problem(result);
        })
        .WithTags(Tags.Clients);
    }
}
