using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Invoices.Queries.GetInvoiceById;

internal sealed class GetInvoiceByIdQueryHandler(
    IApplicationDbContext dbContext)
    : IQueryHandler<GetInvoiceByIdQuery, InvoiceDetailResponse>
{
    public async Task<Result<InvoiceDetailResponse>> Handle(GetInvoiceByIdQuery request, CancellationToken cancellationToken)
    {
        InvoiceDetailResponse? invoice = await dbContext.Invoices
            .AsNoTracking()
            .Where(i => i.Id == request.InvoiceId)
            .Select(i => new InvoiceDetailResponse(
                i.Id,
                i.InvoiceNumber,
                i.ClientId,
                i.Client!.CompanyName,
                i.QuotationId,
                i.IssuedAt,
                i.DueDate,
                i.Status.ToString(),
                i.SubTotalQAR,
                i.TaxAmountQAR,
                i.TotalQAR,
                i.PaidAmountQAR,
                i.OutstandingAmountQAR,
                i.Notes,
                i.LineItems.OrderBy(li => li.SortOrder).Select(li => new InvoiceLineItemResponse(
                    li.Id,
                    li.Description,
                    li.ServiceType.ToString(),
                    li.Quantity,
                    li.UnitPriceQAR,
                    li.DiscountPercent,
                    li.TaxPercent,
                    li.LineTotalQAR,
                    li.SortOrder,
                    li.TripId
                )).ToList(),
                i.TripLinks.Select(tl => new InvoiceTripLinkResponse(
                    tl.Id,
                    tl.TripId,
                    tl.Trip!.TripNumber,
                    tl.TripCompletionVerified
                )).ToList(),
                i.Payments.OrderByDescending(p => p.PaymentDate).Select(p => new InvoicePaymentResponse(
                    p.Id,
                    p.AmountQAR,
                    p.PaymentMethod.ToString(),
                    p.PaymentReference,
                    p.PaymentDate,
                    p.Notes
                )).ToList()
            ))
            .FirstOrDefaultAsync(cancellationToken);

        if (invoice is null)
        {
            return Result.Failure<InvoiceDetailResponse>(Error.NotFound("Invoice.NotFound", "The requested invoice was not found."));
        }

        return invoice;
    }
}
