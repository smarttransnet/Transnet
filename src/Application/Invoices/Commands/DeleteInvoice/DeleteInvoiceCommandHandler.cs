using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Billing;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Invoices.Commands.DeleteInvoice;

internal sealed class DeleteInvoiceCommandHandler(IApplicationDbContext dbContext)
    : ICommandHandler<DeleteInvoiceCommand>
{
    public async Task<Result> Handle(DeleteInvoiceCommand request, CancellationToken cancellationToken)
    {
        Invoice? invoice = await dbContext.Invoices
            .FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken);

        if (invoice is null)
        {
            return Result.Failure(Error.NotFound("Invoice.NotFound", "The invoice was not found."));
        }

        dbContext.Invoices.Remove(invoice);

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
