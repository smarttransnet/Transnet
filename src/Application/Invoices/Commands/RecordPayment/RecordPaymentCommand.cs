using Application.Abstractions.Messaging;
using Domain.Billing.Enums;

namespace Application.Invoices.Commands.RecordPayment;

public sealed record RecordPaymentCommand(
    Guid InvoiceId,
    Guid RecordedByUserId,
    decimal AmountQAR,
    PaymentMethod PaymentMethod,
    DateOnly PaymentDate,
    string? PaymentReference,
    string? Notes
) : ICommand<Guid>;
