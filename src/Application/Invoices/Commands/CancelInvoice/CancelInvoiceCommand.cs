using Application.Abstractions.Messaging;

namespace Application.Invoices.Commands.CancelInvoice;

public record CancelInvoiceCommand(Guid Id) : ICommand;
