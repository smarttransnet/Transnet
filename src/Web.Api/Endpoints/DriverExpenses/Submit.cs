using Application.Abstractions.Messaging;
using Application.Drivers.Expenses.SubmitExpense;
using Domain.Drivers.Enums;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.DriverExpenses;

internal sealed class Submit : IEndpoint
{
    public sealed record Request(Guid? TripId, ExpenseType ExpenseType, decimal Amount, DateOnly ExpenseDate, string Description, Uri? ReceiptUrl, decimal? FuelLitres, string? FuelStation, decimal? OdometerReading);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("drivers/{driverId:guid}/expenses", async (
            Guid driverId,
            Request request,
            ICommandHandler<SubmitExpenseCommand, Guid> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new SubmitExpenseCommand(
                driverId,
                request.TripId,
                request.ExpenseType,
                request.Amount,
                request.ExpenseDate,
                request.Description,
                request.ReceiptUrl,
                request.FuelLitres,
                request.FuelStation,
                request.OdometerReading);

            Result<Guid> result = await handler.Handle(command, cancellationToken);
            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.DriverExpenses);
    }
}
