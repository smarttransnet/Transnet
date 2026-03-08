using Application.Abstractions.Messaging;
using Application.Payroll.Queries.GetSalaryRecords;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Payroll;

internal sealed class GetSalaryRecords : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("payroll/salary-records", async (
            Guid? driverId,
            int? periodMonth,
            int? periodYear,
            IQueryHandler<GetSalaryRecordsQuery, IReadOnlyList<SalaryRecordResponse>> handler,
            CancellationToken cancellationToken) =>
        {
            var query = new GetSalaryRecordsQuery(driverId, periodMonth, periodYear);

            Result<IReadOnlyList<SalaryRecordResponse>> result = await handler.Handle(query, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.Payroll);
    }
}
