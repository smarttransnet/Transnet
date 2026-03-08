using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Clients;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Clients.Commands.RegisterClient;

internal sealed class RegisterClientCommandHandler(
    IApplicationDbContext dbContext)
    : ICommandHandler<RegisterClientCommand, Guid>
{
    public async Task<Result<Guid>> Handle(RegisterClientCommand request, CancellationToken cancellationToken)
    {
        bool codeExists = await dbContext.Clients
            .AnyAsync(c => c.ClientCode == request.ClientCode, cancellationToken);
            
        if (codeExists)
        {
            return Result.Failure<Guid>(Error.Conflict("Client.CodeInUse", "The specified client code is already in use."));
        }

        var client = new Client
        {
            Id = Guid.NewGuid(),
            ClientCode = request.ClientCode,
            CompanyName = request.CompanyName,
            ContactPersonName = request.ContactPersonName,
            ContactEmail = request.ContactEmail,
            ContactPhone = request.ContactPhone,
            BillingAddress = request.BillingAddress,
            TaxRegistrationNumber = request.TaxRegistrationNumber,
            PaymentTermsDays = request.PaymentTermsDays,
            CurrencyCode = string.IsNullOrWhiteSpace(request.CurrencyCode) ? "QAR" : request.CurrencyCode,
            IsActive = true
        };

        dbContext.Clients.Add(client);
        await dbContext.SaveChangesAsync(cancellationToken);

        return client.Id;
    }
}
