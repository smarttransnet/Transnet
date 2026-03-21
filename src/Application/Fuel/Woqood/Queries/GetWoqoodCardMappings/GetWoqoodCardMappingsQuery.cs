using Application.Abstractions.Messaging;

namespace Application.Fuel.Woqood.Queries.GetWoqoodCardMappings;

public sealed record GetWoqoodCardMappingsQuery(
    Guid? ClientId = null,
    string? SearchTerm = null
) : IQuery<IReadOnlyList<WoqoodCardMappingResponse>>;
