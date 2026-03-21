using Application.Abstractions.Messaging;
using Application.Fuel.Woqood;
using Application.Fuel.Woqood.Queries.GetWoqoodCardMappings;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.WoqoodCardMappings;

internal sealed class GetCardMappings : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("fuel/woqood/card-mappings", async (
            IQueryHandler<GetWoqoodCardMappingsQuery, IReadOnlyList<WoqoodCardMappingResponse>> handler,
            CancellationToken cancellationToken) =>
        {
            var query = new GetWoqoodCardMappingsQuery();

            Result<IReadOnlyList<WoqoodCardMappingResponse>> result = await handler.Handle(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.WoqoodCardMappings);
    }
}
