using Application.Abstractions.Messaging;

namespace Application.Clients.Commands.RegisterClient;

public sealed record RegisterClientCommand(
    string ClientCode,
    string CompanyName,
    string ContactPersonName,
    string ContactEmail,
    string ContactPhone,
    string BillingAddress,
    string? TaxRegistrationNumber,
    int PaymentTermsDays,
    string CurrencyCode
) : ICommand<Guid>;
