using GenesysForge.Domain.Users;

namespace GenesysForge.Application.Auth;

public interface IAccessTokenService
{
    AccessToken CreateAccessToken(User user);
}
