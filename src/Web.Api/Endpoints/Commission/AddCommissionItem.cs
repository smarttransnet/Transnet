using Application.Abstractions.Messaging;
using Application.Payroll.Commands.AddCommissionItem;
using Domain.Payroll.Enums;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.Commission;

public sealed record AddCommissionItemRequest(
    Guid DriverSalaryRecordId,
    Guid? TripId,
    CommissionType CommissionType,
    string Description,
    decimal AmountQAR,
    string? CalculationBasis
);

internal sealed class AddCommissionItem : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("payroll/commission-items", async (
            AddCommissionItemRequest request,
            ICommandHandler<AddCommissionItemCommand, Guid> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new AddCommissionItemCommand(
                request.DriverSalaryRecordId,
                request.TripId,
                request.CommissionType,
                request.Description,
                request.AmountQAR,
                request.CalculationBasis
            );

            Result<Guid> result = await handler.Handle(command, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.Commission);
    }
}
