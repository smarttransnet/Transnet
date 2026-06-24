using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Billing;
using Domain.Billing.Enums;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Invoices.Commands.RecordPayment;

internal sealed class RecordPaymentCommandHandler(
    IApplicationDbContext dbContext)
    : ICommandHandler<RecordPaymentCommand, Guid>
{
    public async Task<Result<Guid>> Handle(RecordPaymentCommand request, CancellationToken cancellationToken)
    {
        // 1. Fetch the invoice. We don't necessarily need .Include(i => i.Payments) if we add to DbSet directly,
        // which avoids common EF Core collection synchronization issues that lead to concurrency errors.
        Invoice? invoice = await dbContext.Invoices
            .Include(i => i.Payments)
            .FirstOrDefaultAsync(i => i.Id == request.InvoiceId, cancellationToken);

        if (invoice is null)
        {
            return Result.Failure<Guid>(Error.NotFound("Invoice.NotFound", "The specified invoice was not found."));
        }

        // Initialize collection if EF materialization somehow resulted in null
        invoice.Payments ??= new List<InvoicePayment>();

        if (invoice.OutstandingAmountQAR <= 0)
        {
            return Result.Failure<Guid>(Error.Problem("Invoice.AlreadyPaid", "This invoice is already fully paid."));
        }

        // 2. Create the payment record
        var payment = new InvoicePayment
        {
            Id = Guid.NewGuid(),
            InvoiceId = request.InvoiceId,
            AmountQAR = request.AmountQAR,
            PaymentMethod = request.PaymentMethod,
            PaymentReference = request.PaymentReference,
            PaymentDate = request.PaymentDate,
            RecordedByUserId = request.RecordedByUserId,
            Notes = request.Notes
        };

        // 3. Add payment directly to the context and update invoice status
        dbContext.InvoicePayments.Add(payment);

        invoice.PaidAmountQAR += request.AmountQAR;
        invoice.OutstandingAmountQAR -= request.AmountQAR;

        if (invoice.OutstandingAmountQAR <= 0)
        {
            invoice.OutstandingAmountQAR = 0;
            invoice.Status = InvoiceStatus.Paid;
        }
        else
        {
            invoice.Status = InvoiceStatus.PartiallyPaid;
        }

        try
        {
            await dbContext.SaveChangesAsync(cancellationToken);
            return payment.Id;
        }
        catch (DbUpdateConcurrencyException ex)
        {
            // If it fails with "0 rows affected", it's likely because the Invoice row 
            // state changed or was deleted. We re-throw with more context if needed,
            // or return a failure.
            return Result.Failure<Guid>(Error.Problem("Invoice.ConcurrencyError", 
                "The invoice was modified by another process. Please refresh and try again. " + ex.Message));
        }
        catch (Exception ex)
        {
            return Result.Failure<Guid>(Error.Failure("Invoice.PaymentError", 
                "An unexpected error occurred while saving the payment: " + ex.Message));
        }
    }
}
