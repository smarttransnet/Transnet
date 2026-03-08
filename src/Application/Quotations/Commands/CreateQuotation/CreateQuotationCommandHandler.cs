using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Billing;
using Domain.Billing.Enums;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Quotations.Commands.CreateQuotation;

internal sealed class CreateQuotationCommandHandler(
    IApplicationDbContext dbContext)
    : ICommandHandler<CreateQuotationCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateQuotationCommand request, CancellationToken cancellationToken)
    {
        bool clientExists = await dbContext.Clients.AnyAsync(c => c.Id == request.ClientId, cancellationToken);
        if (!clientExists)
        {
            return Result.Failure<Guid>(Error.NotFound("Client.NotFound", "The specified client was not found."));
        }

        string datePrefix = DateTime.UtcNow.ToString("yyyyMM", System.Globalization.CultureInfo.InvariantCulture);
        int count = await dbContext.Quotations.CountAsync(q => q.QuotationNumber.StartsWith($"QT-{datePrefix}"), cancellationToken);
        string quotationNumber = $"QT-{datePrefix}-{(count + 1).ToString(System.Globalization.CultureInfo.InvariantCulture).PadLeft(4, '0')}";

        var quotation = new Quotation
        {
            Id = Guid.NewGuid(),
            QuotationNumber = quotationNumber,
            ClientId = request.ClientId,
            IssuedByUserId = request.IssuedByUserId,
            IssuedAt = DateTime.UtcNow,
            ValidUntil = request.ValidUntil,
            Status = QuotationStatus.Draft,
            Notes = request.Notes,
            TermsAndConditions = request.TermsAndConditions
        };

        decimal subTotal = 0m;
        decimal taxAmount = 0m;

        foreach (QuotationLineItemRequest item in request.LineItems)
        {
            decimal lineGross = item.Quantity * item.UnitPriceQAR;
            decimal discountAmount = lineGross * (item.DiscountPercent / 100m);
            decimal lineNet = lineGross - discountAmount;
            decimal lineTax = lineNet * (item.TaxPercent / 100m);
            decimal lineTotal = lineNet + lineTax;

            subTotal += lineNet;
            taxAmount += lineTax;

            quotation.LineItems.Add(new QuotationLineItem
            {
                Id = Guid.NewGuid(),
                QuotationId = quotation.Id,
                Description = item.Description,
                ServiceType = item.ServiceType,
                Quantity = item.Quantity,
                UnitPriceQAR = item.UnitPriceQAR,
                DiscountPercent = item.DiscountPercent,
                TaxPercent = item.TaxPercent,
                LineTotalQAR = lineTotal,
                SortOrder = item.SortOrder
            });
        }

        quotation.SubTotalQAR = subTotal;
        quotation.TaxAmountQAR = taxAmount;
        quotation.TotalQAR = subTotal + taxAmount;

        dbContext.Quotations.Add(quotation);
        await dbContext.SaveChangesAsync(cancellationToken);

        return quotation.Id;
    }
}
