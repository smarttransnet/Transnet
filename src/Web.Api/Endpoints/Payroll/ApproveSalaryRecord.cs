using Application.Abstractions.Messaging;
using Application.Payroll.Commands.ApproveSalaryRecord;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Payroll;

internal sealed class ApproveSalaryRecord : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("payroll/salary-records/{id:guid}/approve", async (
            Guid id,
            ICommandHandler<ApproveSalaryRecordCommand> handler,
            CancellationToken cancellationToken) =>
        {
            var mockSponsorId = Guid.NewGuid();

            var command = new ApproveSalaryRecordCommand(id, mockSponsorId);

            Result result = await handler.Handle(command, cancellationToken);

            return result.Match(() => Results.Ok(), CustomResults.Problem);
        })
        .WithTags(Tags.Payroll);
    }
}
