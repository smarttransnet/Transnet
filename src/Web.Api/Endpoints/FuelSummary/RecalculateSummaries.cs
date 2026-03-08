using Application.Abstractions.Messaging;
using Application.Fuel.Summaries.Commands.RecalculateFuelSummary;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Endpoints.FuelSummary;

public sealed record RecalculateSummariesRequest(
    int PeriodMonth,
    int PeriodYear
);

internal sealed class RecalculateSummaries : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("fuel/vehicle-summary/recalculate", async (
            RecalculateSummariesRequest request,
            ICommandHandler<RecalculateFuelSummaryCommand> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new RecalculateFuelSummaryCommand(
                request.PeriodMonth,
                request.PeriodYear
            );

            Result result = await handler.Handle(command, cancellationToken);

            return result.Match(() => Results.Ok(), CustomResults.Problem);
        })
        .WithTags(Tags.FuelSummary);
    }
}
