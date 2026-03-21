using Application.Abstractions.Messaging;
using Application.Fuel.Woqood;
using Application.Fuel.Woqood.Queries.GetWoqoodCardMappingById;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.WoqoodCardMappings;

internal sealed class GetById : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("woqood-card-mappings/{id:guid}", async (
            Guid id,
            IQueryHandler<GetWoqoodCardMappingByIdQuery, WoqoodCardMappingResponse> handler,
            CancellationToken cancellationToken) =>
        {
            var query = new GetWoqoodCardMappingByIdQuery(id);

            Result<WoqoodCardMappingResponse> result = await handler.Handle(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.WoqoodCardMappings);
    }
}
