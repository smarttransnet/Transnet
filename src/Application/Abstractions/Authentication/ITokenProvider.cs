using Domain.Users;

namespace Application.Abstractions.Authentication;

public interface ITokenProvider
{
    string Create(User user);
    string CreateForDriver(Domain.Drivers.Driver driver);
    string CreateForClientPortalUser(Domain.Clients.ClientPortalUser user);
    string CreateRefreshToken();
}
