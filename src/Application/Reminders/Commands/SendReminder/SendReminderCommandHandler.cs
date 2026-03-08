using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Billing;
using Domain.Billing.Enums;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Reminders.Commands.SendReminder;

internal sealed class SendReminderCommandHandler(
    IApplicationDbContext dbContext)
    : ICommandHandler<SendReminderCommand, Guid>
{
    public async Task<Result<Guid>> Handle(SendReminderCommand request, CancellationToken cancellationToken)
    {
        Invoice? invoice = await dbContext.Invoices
            .FirstOrDefaultAsync(i => i.Id == request.InvoiceId, cancellationToken);

        if (invoice is null)
        {
            return Result.Failure<Guid>(Error.NotFound("Invoice.NotFound", "The specified invoice was not found."));
        }

        var log = new InvoiceReminderLog
        {
            Id = Guid.NewGuid(),
            InvoiceId = request.InvoiceId,
            SentAt = DateTime.UtcNow,
            SentToEmail = request.SentToEmail,
            ReminderType = request.ReminderType,
            DeliveryStatus = ReminderDeliveryStatus.Pending, // Will be updated by webhook later
            TriggeredByUserId = request.TriggeredByUserId,
            IsAutomated = !request.TriggeredByUserId.HasValue
        };

        // Here we'd integrate with an IEmailService or similar to actually send it via SendGrid/etc

        dbContext.InvoiceReminderLogs.Add(log);
        await dbContext.SaveChangesAsync(cancellationToken);

        return log.Id;
    }
}
