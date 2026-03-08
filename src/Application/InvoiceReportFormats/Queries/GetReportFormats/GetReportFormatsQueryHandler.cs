using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.InvoiceReportFormats.Queries.GetReportFormats;

internal sealed class GetReportFormatsQueryHandler(
    IApplicationDbContext dbContext)
    : IQueryHandler<GetReportFormatsQuery, IReadOnlyList<ReportFormatResponse>>
{
    public async Task<Result<IReadOnlyList<ReportFormatResponse>>> Handle(GetReportFormatsQuery request, CancellationToken cancellationToken)
    {
        IQueryable<Domain.Billing.InvoiceReportFormat> query = dbContext.InvoiceReportFormats.AsNoTracking();

        if (request.IsActive.HasValue)
        {
            query = query.Where(rf => rf.IsActive == request.IsActive.Value);
        }

        List<ReportFormatResponse> formats = await query
            .OrderByDescending(rf => rf.IsDefault)
            .ThenBy(rf => rf.Name)
            .Select(rf => new ReportFormatResponse(
                rf.Id,
                rf.Name,
                rf.Description,
                rf.ShowShippingAddress,
                rf.ShowTaxBreakdown,
                rf.ShowTripDetails,
                rf.ColumnConfiguration,
                rf.IsDefault,
                rf.IsActive
            ))
            .ToListAsync(cancellationToken);

        return formats;
    }
}
