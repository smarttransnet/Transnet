using Application.Abstractions.Messaging;

namespace Application.Invoices.Commands.IssueInvoice;

public sealed record IssueInvoiceCommand(Guid Id) : ICommand;
