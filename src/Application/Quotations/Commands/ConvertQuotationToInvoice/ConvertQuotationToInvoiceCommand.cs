using Application.Abstractions.Messaging;

namespace Application.Quotations.Commands.ConvertQuotationToInvoice;

public sealed record ConvertQuotationToInvoiceCommand(
    Guid QuotationId,
    Guid IssuedByUserId,
    DateOnly DueDate
) : ICommand<Guid>;
