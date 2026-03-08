using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Clients;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Clients.Commands.UpdateClient;

internal sealed class UpdateClientCommandHandler(
    IApplicationDbContext dbContext)
    : ICommandHandler<UpdateClientCommand>
{
    public async Task<Result> Handle(UpdateClientCommand request, CancellationToken cancellationToken)
    {
        Client? client = await dbContext.Clients
            .FirstOrDefaultAsync(c => c.Id == request.ClientId, cancellationToken);

        if (client is null)
        {
            return Result.Failure(Error.NotFound("Client.NotFound", "The requested client was not found."));
        }

        client.CompanyName = request.CompanyName;
        client.ContactPersonName = request.ContactPersonName;
        client.ContactEmail = request.ContactEmail;
        client.ContactPhone = request.ContactPhone;
        client.BillingAddress = request.BillingAddress;
        client.TaxRegistrationNumber = request.TaxRegistrationNumber;
        client.PaymentTermsDays = request.PaymentTermsDays;
        client.CurrencyCode = request.CurrencyCode;
        client.IsActive = request.IsActive;

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
