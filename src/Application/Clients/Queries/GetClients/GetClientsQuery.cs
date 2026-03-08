using Application.Abstractions.Messaging;

namespace Application.Clients.Queries.GetClients;

public sealed record GetClientsQuery(
    string? SearchTerm = null,
    bool? IsActive = null
) : IQuery<IReadOnlyList<ClientResponse>>;
