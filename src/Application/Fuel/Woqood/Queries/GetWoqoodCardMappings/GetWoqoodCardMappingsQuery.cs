using Application.Abstractions.Messaging;

namespace Application.Fuel.Woqood.Queries.GetWoqoodCardMappings;

public sealed record GetWoqoodCardMappingsQuery : IQuery<IReadOnlyList<WoqoodCardMappingResponse>>;
