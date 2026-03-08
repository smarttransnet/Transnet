using Application.Abstractions.Messaging;

namespace Application.Clients.Queries.GetPortalUsers;

public sealed record GetPortalUsersQuery(
    Guid ClientId,
    bool? IsActive = null
) : IQuery<IReadOnlyList<ClientPortalUserResponse>>;
