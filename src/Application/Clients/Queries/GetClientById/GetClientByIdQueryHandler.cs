using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Clients.Queries.GetClientById;

internal sealed class GetClientByIdQueryHandler(
    IApplicationDbContext dbContext)
    : IQueryHandler<GetClientByIdQuery, Application.Clients.Queries.GetClients.ClientResponse>
{
    public async Task<Result<Application.Clients.Queries.GetClients.ClientResponse>> Handle(GetClientByIdQuery request, CancellationToken cancellationToken)
    {
        Application.Clients.Queries.GetClients.ClientResponse? client = await dbContext.Clients
            .Where(c => c.Id == request.ClientId)
            .Select(c => new Application.Clients.Queries.GetClients.ClientResponse(
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
            .FirstOrDefaultAsync(cancellationToken);

        if (client is null)
        {
            return Result.Failure<Application.Clients.Queries.GetClients.ClientResponse>(Error.NotFound("Client.NotFound", "The requested client was not found."));
        }

        return client;
    }
}
