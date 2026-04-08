using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Billing;
using Domain.Billing.Enums;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Invoices.Commands.IssueInvoice;

internal sealed class IssueInvoiceCommandHandler(IApplicationDbContext dbContext)
    : ICommandHandler<IssueInvoiceCommand>
{
    public async Task<Result> Handle(IssueInvoiceCommand request, CancellationToken cancellationToken)
    {
        Invoice? invoice = await dbContext.Invoices
            .FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken);

        if (invoice is null)
        {
            return Result.Failure(Error.NotFound("Invoice.NotFound", "The invoice was not found."));
        }

        if (invoice.Status != InvoiceStatus.Draft)
        {
            return Result.Failure(Error.Problem("Invoice.NotDraft", "Only draft invoices can be issued."));
        }

        invoice.Status = InvoiceStatus.Issued;
        invoice.IssuedAt = DateTime.UtcNow;

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
