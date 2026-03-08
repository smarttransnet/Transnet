using Application.Abstractions.Messaging;

namespace Application.Payroll.Commands.SubmitSalaryRecord;

public sealed record SubmitSalaryRecordCommand(Guid SalaryRecordId) : ICommand;
