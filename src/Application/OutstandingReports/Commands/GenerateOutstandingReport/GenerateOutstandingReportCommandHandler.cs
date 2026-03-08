using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Billing;
using Domain.Billing.Enums;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.OutstandingReports.Commands.GenerateOutstandingReport;

internal sealed class GenerateOutstandingReportCommandHandler(
    IApplicationDbContext dbContext)
    : ICommandHandler<GenerateOutstandingReportCommand, Guid>
{
    public async Task<Result<Guid>> Handle(GenerateOutstandingReportCommand request, CancellationToken cancellationToken)
    {
        bool clientExists = await dbContext.Clients.AnyAsync(c => c.Id == request.ClientId, cancellationToken);
        if (!clientExists)
        {
            return Result.Failure<Guid>(Error.NotFound("Client.NotFound", "The specified client was not found."));
        }

        List<Invoice> outstandingInvoices = await dbContext.Invoices
            .Where(i => i.ClientId == request.ClientId && i.Status != InvoiceStatus.Paid && i.Status != InvoiceStatus.Cancelled && i.Status != InvoiceStatus.Draft)
            .ToListAsync(cancellationToken);

        var report = new OutstandingInvoiceReport
        {
            Id = Guid.NewGuid(),
            ClientId = request.ClientId,
            GeneratedAt = DateTime.UtcNow,
            PeriodMonth = request.PeriodMonth,
            PeriodYear = request.PeriodYear,
            InvoiceCount = outstandingInvoices.Count,
            TotalOutstandingQAR = outstandingInvoices.Sum(i => i.OutstandingAmountQAR),
            DeliveryStatus = ReminderDeliveryStatus.Pending
        };

        if (outstandingInvoices.Count > 0)
        {
            report.OldestInvoiceDate = outstandingInvoices.Min(i => i.IssuedAt).Date.ToDateOnly();
        }

        var today = DateOnly.FromDateTime(DateTime.UtcNow);

        foreach (Invoice invoice in outstandingInvoices)
        {
            int agingDays = invoice.DueDate < today ? today.DayNumber - invoice.DueDate.DayNumber : 0;
            AgingBucket bucket = GetAgingBucket(agingDays);

            report.Snapshots.Add(new OutstandingInvoiceSnapshot
            {
                Id = Guid.NewGuid(),
                OutstandingInvoiceReportId = report.Id,
                InvoiceId = invoice.Id,
                InvoiceNumber = invoice.InvoiceNumber,
                IssuedAt = invoice.IssuedAt,
                DueDate = invoice.DueDate,
                OriginalAmountQAR = invoice.TotalQAR,
                OutstandingAmountQAR = invoice.OutstandingAmountQAR,
                AgingDays = agingDays,
                AgingBucket = bucket
            });
        }

        dbContext.OutstandingInvoiceReports.Add(report);
        await dbContext.SaveChangesAsync(cancellationToken);

        return report.Id;
    }

    private static AgingBucket GetAgingBucket(int agingDays)
    {
        if (agingDays <= 0) 
        {
            return AgingBucket.Current;
        }
        if (agingDays <= 30) 
        {
            return AgingBucket.Days1to30;
        }
        if (agingDays <= 60) 
        {
            return AgingBucket.Days31to60;
        }
        if (agingDays <= 90) 
        {
            return AgingBucket.Days61to90;
        }
        return AgingBucket.Over90Days;
    }
}

internal static class DateExtensions
{
    public static DateOnly ToDateOnly(this DateTime dt)
    {
        return DateOnly.FromDateTime(dt);
    }
}
