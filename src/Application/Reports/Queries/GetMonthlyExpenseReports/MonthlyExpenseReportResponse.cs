namespace Application.Reports.Queries.GetMonthlyExpenseReports;

public sealed record MonthlyExpenseReportResponse(
    Guid Id,
    int PeriodMonth,
    int PeriodYear,
    string ReportType,
    decimal TotalFuelCostQAR,
    decimal TotalSalaryCostQAR,
    decimal TotalDriverExpensesQAR,
    decimal TotalOperationalCostQAR,
    string Status,
    string? ExportedFilePath,
    DateTime GeneratedAt
);
