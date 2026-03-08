namespace Application.Clients.Queries.GetPortalUsers;

public sealed record ClientPortalUserResponse(
    Guid Id,
    Guid ClientId,
    string FullName,
    string Email,
    string Role,
    bool IsActive,
    DateTime? LastLoginAt
);
