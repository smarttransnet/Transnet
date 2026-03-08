namespace Application.Clients.Queries.GetClients;

public sealed record ClientResponse(
    Guid Id,
    string ClientCode,
    string CompanyName,
    string ContactPersonName,
    string ContactEmail,
    string ContactPhone,
    string BillingAddress,
    string? TaxRegistrationNumber,
    int PaymentTermsDays,
    string CurrencyCode,
    bool IsActive
);
