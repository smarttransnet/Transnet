using Application.Abstractions.Messaging;
using Application.Payroll.Commands.CreateSalaryRecord;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Payroll;

public sealed record CreateSalaryRecordRequest(
    Guid DriverId,
    int PeriodMonth,
    int PeriodYear,
    decimal BaseSalaryQAR,
    decimal AllowancesQAR,
    string? Notes
);

internal sealed class CreateSalaryRecord : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("payroll/salary-records", async (
            CreateSalaryRecordRequest request,
            ICommandHandler<CreateSalaryRecordCommand, Guid> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new CreateSalaryRecordCommand(
                request.DriverId,
                request.PeriodMonth,
                request.PeriodYear,
                request.BaseSalaryQAR,
                request.AllowancesQAR,
                request.Notes
            );

            Result<Guid> result = await handler.Handle(command, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.Payroll);
    }
}
