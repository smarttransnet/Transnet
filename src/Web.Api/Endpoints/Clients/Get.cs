using Application.Abstractions.Messaging;
using Application.Clients.Commands.RegisterClient;
using Application.Clients.Queries.GetClientById;
using Application.Clients.Queries.GetClients;
using MediatR;
using Microsoft.AspNetCore.Http;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Clients;

internal sealed class Get : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("clients", async (
            IQueryHandler<GetClientsQuery, IReadOnlyList<ClientResponse>> handler,
            CancellationToken cancellationToken) =>
        {
            var query = new GetClientsQuery();
            Result<IReadOnlyList<ClientResponse>> result = await handler.Handle(query, cancellationToken);

            return result.IsSuccess ? Results.Ok(result.Value) : CustomResults.Problem(result);
        })
        .WithTags(Tags.Clients);
    }
}
