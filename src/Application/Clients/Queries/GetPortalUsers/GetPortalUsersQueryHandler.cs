using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Clients.Queries.GetPortalUsers;

internal sealed class GetPortalUsersQueryHandler(
    IApplicationDbContext dbContext)
    : IQueryHandler<GetPortalUsersQuery, IReadOnlyList<ClientPortalUserResponse>>
{
    public async Task<Result<IReadOnlyList<ClientPortalUserResponse>>> Handle(GetPortalUsersQuery request, CancellationToken cancellationToken)
    {
        IQueryable<Domain.Clients.ClientPortalUser> query = dbContext.ClientPortalUsers
            .AsNoTracking()
            .Where(u => u.ClientId == request.ClientId);

        if (request.IsActive.HasValue)
        {
            query = query.Where(u => u.IsActive == request.IsActive.Value);
        }

        List<ClientPortalUserResponse> users = await query
            .OrderBy(u => u.FullName)
            .Select(u => new ClientPortalUserResponse(
                u.Id,
                u.ClientId,
                u.FullName,
                u.Email,
                u.Role.ToString(),
                u.IsActive,
                u.LastLoginAt
            ))
            .ToListAsync(cancellationToken);

        return users;
    }
}
