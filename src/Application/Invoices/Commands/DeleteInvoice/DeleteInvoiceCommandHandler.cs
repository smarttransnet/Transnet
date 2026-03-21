using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Billing;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Invoices.Commands.DeleteInvoice;

internal sealed class DeleteInvoiceCommandHandler(IApplicationDbContext context)
    : ICommandHandler<DeleteInvoiceCommand>
{
    public async Task<Result> Handle(DeleteInvoiceCommand command, CancellationToken cancellationToken)
    {
        Invoice? invoice = await context.Invoices
            .Include(i => i.LineItems)
            .Include(i => i.Payments)
            .Include(i => i.ReminderLogs)
            .Include(i => i.TripLinks)
            .FirstOrDefaultAsync(i => i.Id == command.Id, cancellationToken);

        if (invoice is null)
        {
            return Result.Failure(Error.NotFound("Invoices.NotFound", $"The invoice with the Id = '{command.Id}' was not found"));
        }

        context.Invoices.Remove(invoice);

        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
