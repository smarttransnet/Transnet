using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Billing;
using Domain.Billing.Enums;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Invoices.Commands.CancelInvoice;

internal sealed class CancelInvoiceCommandHandler(IApplicationDbContext dbContext)
    : ICommandHandler<CancelInvoiceCommand>
{
    public async Task<Result> Handle(CancelInvoiceCommand request, CancellationToken cancellationToken)
    {
        Invoice? invoice = await dbContext.Invoices
            .FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken);

        if (invoice is null)
        {
            return Result.Failure(Error.NotFound("Invoice.NotFound", "The invoice was not found."));
        }

        if (invoice.Status == InvoiceStatus.Paid || invoice.Status == InvoiceStatus.PartiallyPaid)
        {
            return Result.Failure(Error.Problem("Invoice.CannotCancel", "Paid or partially paid invoices cannot be cancelled."));
        }

        invoice.Status = InvoiceStatus.Cancelled;

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
