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
        Invoice? invoice = await dbContext.Invoices
            .FirstOrDefaultAsync(i => i.Id == request.InvoiceId, cancellationToken);

        if (invoice is null)
        {
            return Result.Failure<Guid>(Error.NotFound("Invoice.NotFound", "The specified invoice was not found."));
        }

        if (invoice.OutstandingAmountQAR <= 0)
        {
            return Result.Failure<Guid>(Error.Problem("Invoice.AlreadyPaid", "This invoice is already fully paid."));
        }

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

        invoice.Payments.Add(payment);
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

        await dbContext.SaveChangesAsync(cancellationToken);

        return payment.Id;
    }
}
