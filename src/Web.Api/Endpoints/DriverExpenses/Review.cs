using Application.Abstractions.Messaging;
using Application.Drivers.Expenses.ReviewExpense;
using Domain.Drivers.Enums;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.DriverExpenses;

internal sealed class Review : IEndpoint
{
    public sealed record Request(ExpenseStatus Status, string? ReviewNotes);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("drivers/expenses/{id:guid}/review", async (
            Guid id,
            Request request,
            ICommandHandler<ReviewExpenseCommand> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new ReviewExpenseCommand(id, request.Status, request.ReviewNotes);
            Result result = await handler.Handle(command, cancellationToken);
            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithTags(Tags.DriverExpenses);
    }
}
