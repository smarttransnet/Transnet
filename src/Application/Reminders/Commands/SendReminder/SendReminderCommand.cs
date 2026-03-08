using Application.Abstractions.Messaging;
using Domain.Billing.Enums;

namespace Application.Reminders.Commands.SendReminder;

public sealed record SendReminderCommand(
    Guid InvoiceId,
    string SentToEmail,
    ReminderType ReminderType,
    Guid? TriggeredByUserId
) : ICommand<Guid>;
