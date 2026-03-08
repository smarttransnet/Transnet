using Application.Abstractions.Messaging;

namespace Application.Payroll.Commands.ApproveSalaryRecord;

public sealed record ApproveSalaryRecordCommand(
    Guid SalaryRecordId,
    Guid SponsorId
) : ICommand;
