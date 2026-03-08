using Application.Abstractions.Messaging;

namespace Application.Fuel.Summaries.Commands.RecalculateFuelSummary;

public sealed record RecalculateFuelSummaryCommand(
    int PeriodMonth,
    int PeriodYear
) : ICommand;
