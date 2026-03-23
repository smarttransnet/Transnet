using Application.Abstractions.Messaging;

namespace Application.Invoices.Commands.DeleteInvoice;

public sealed record DeleteInvoiceCommand(Guid Id) : ICommand;
