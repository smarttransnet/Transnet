using Application.Abstractions.Messaging;

namespace Application.Invoices.Commands.LinkTripToInvoice;

public sealed record LinkTripToInvoiceCommand(
    Guid InvoiceId,
    Guid TripId,
    Guid LinkedByUserId
) : ICommand;
