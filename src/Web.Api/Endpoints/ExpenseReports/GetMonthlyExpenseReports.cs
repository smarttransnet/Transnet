using Application.Abstractions.Messaging;
using Application.Reports.Queries.GetMonthlyExpenseReports;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.ExpenseReports;

internal sealed class GetMonthlyExpenseReports : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("reports/monthly-expenses", async (
            int? periodMonth,
            int? periodYear,
            IQueryHandler<GetMonthlyExpenseReportsQuery, IReadOnlyList<MonthlyExpenseReportResponse>> handler,
            CancellationToken cancellationToken) =>
        {
            var query = new GetMonthlyExpenseReportsQuery(periodMonth, periodYear);

            Result<IReadOnlyList<MonthlyExpenseReportResponse>> result = await handler.Handle(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.ExpenseReports);
    }
}
