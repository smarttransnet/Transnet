using Application.Abstractions.Messaging;

namespace Application.Quotations.Commands.DeleteQuotation;

public sealed record DeleteQuotationCommand(Guid Id) : ICommand;
