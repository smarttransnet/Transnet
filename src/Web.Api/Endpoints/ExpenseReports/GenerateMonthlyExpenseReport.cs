using Application.Abstractions.Messaging;
using Application.Reports.Commands.GenerateMonthlyExpenseReport;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.ExpenseReports;

public sealed record GenerateMonthlyExpenseReportRequest(
    int PeriodMonth,
    int PeriodYear
);

internal sealed class GenerateMonthlyExpenseReport : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("reports/monthly-expenses/generate", async (
            GenerateMonthlyExpenseReportRequest request,
            ICommandHandler<GenerateMonthlyExpenseReportCommand, Guid> handler,
            CancellationToken cancellationToken) =>
        {
            var mockUserId = Guid.NewGuid();

            var command = new GenerateMonthlyExpenseReportCommand(
                request.PeriodMonth,
                request.PeriodYear,
                mockUserId
            );

            Result<Guid> result = await handler.Handle(command, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.ExpenseReports);
    }
}
