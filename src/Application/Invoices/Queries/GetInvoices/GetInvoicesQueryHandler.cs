using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Invoices.Queries.GetInvoices;

internal sealed class GetInvoicesQueryHandler(
    IApplicationDbContext dbContext)
    : IQueryHandler<GetInvoicesQuery, IReadOnlyList<InvoiceResponse>>
{
    public async Task<Result<IReadOnlyList<InvoiceResponse>>> Handle(GetInvoicesQuery request, CancellationToken cancellationToken)
    {
        IQueryable<Domain.Billing.Invoice> query = dbContext.Invoices.AsNoTracking();

        if (request.ClientId.HasValue)
        {
            query = query.Where(i => i.ClientId == request.ClientId.Value);
        }

        if (request.Status.HasValue)
        {
            query = query.Where(i => i.Status == request.Status.Value);
        }

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            query = query.Where(i => i.InvoiceNumber.Contains(request.SearchTerm));
        }

        IReadOnlyList<InvoiceResponse> invoices = await query
            .OrderByDescending(i => i.IssuedAt)
            .Include(i => i.Client)
            .Select(i => new InvoiceResponse(
                i.Id,
                i.InvoiceNumber,
                i.ClientId,
                i.Client!.CompanyName,
                i.IssuedAt,
                i.DueDate,
                i.Status.ToString(),
                i.TotalQAR,
                i.PaidAmountQAR,
                i.OutstandingAmountQAR
            ))
            .ToListAsync(cancellationToken);

        return Result.Success(invoices);
    }
}
