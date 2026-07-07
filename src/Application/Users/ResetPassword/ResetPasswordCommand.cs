using Application.Abstractions.Messaging;

namespace Application.Users.ResetPassword;

public sealed record ResetPasswordCommand(string Email, string NewPassword) : ICommand;
