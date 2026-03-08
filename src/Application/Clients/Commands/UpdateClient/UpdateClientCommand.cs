using Application.Abstractions.Messaging;

namespace Application.Clients.Commands.UpdateClient;

public sealed record UpdateClientCommand(
    Guid ClientId,
    string CompanyName,
    string ContactPersonName,
    string ContactEmail,
    string ContactPhone,
    string BillingAddress,
    string? TaxRegistrationNumber,
    int PaymentTermsDays,
    string CurrencyCode,
    bool IsActive
) : ICommand;
