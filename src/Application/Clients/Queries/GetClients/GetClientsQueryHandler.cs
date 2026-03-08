using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Clients.Queries.GetClients;

internal sealed class GetClientsQueryHandler(
    IApplicationDbContext dbContext)
    : IQueryHandler<GetClientsQuery, IReadOnlyList<ClientResponse>>
{
    public async Task<Result<IReadOnlyList<ClientResponse>>> Handle(GetClientsQuery request, CancellationToken cancellationToken)
    {
        IQueryable<Domain.Clients.Client> query = dbContext.Clients.AsNoTracking();

        if (request.IsActive.HasValue)
        {
            query = query.Where(c => c.IsActive == request.IsActive.Value);
        }

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            query = query.Where(c => 
                c.ClientCode.Contains(request.SearchTerm) || 
                c.CompanyName.Contains(request.SearchTerm) ||
                c.ContactEmail.Contains(request.SearchTerm));
        }

        List<ClientResponse> clients = await query
            .OrderBy(c => c.ClientCode)
            .Select(c => new ClientResponse(
                c.Id,
                c.ClientCode,
                c.CompanyName,
                c.ContactPersonName,
                c.ContactEmail,
                c.ContactPhone,
                c.BillingAddress,
                c.TaxRegistrationNumber,
                c.PaymentTermsDays,
                c.CurrencyCode,
                c.IsActive
            ))
            .ToListAsync(cancellationToken);

        return clients;
    }
}
