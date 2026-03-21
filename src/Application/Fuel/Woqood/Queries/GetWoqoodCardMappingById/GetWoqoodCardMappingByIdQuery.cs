using Application.Abstractions.Messaging;

namespace Application.Fuel.Woqood.Queries.GetWoqoodCardMappingById;

public sealed record GetWoqoodCardMappingByIdQuery(Guid Id) : IQuery<WoqoodCardMappingResponse>;
