using Application.Abstractions.Messaging;
using Application.Payroll.Commands.SubmitSalaryRecord;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Payroll;

internal sealed class SubmitSalaryRecord : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("payroll/salary-records/{id:guid}/submit", async (
            Guid id,
            ICommandHandler<SubmitSalaryRecordCommand> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new SubmitSalaryRecordCommand(id);

            Result result = await handler.Handle(command, cancellationToken);

            return result.Match(() => Results.Ok(), CustomResults.Problem);
        })
        .WithTags(Tags.Payroll);
    }
}
