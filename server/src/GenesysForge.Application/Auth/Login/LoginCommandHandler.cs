using GenesysForge.Contracts.Auth;
using GenesysForge.Domain.Users;
using MediatR;

namespace GenesysForge.Application.Auth.Login;

public sealed class LoginCommandHandler(
    IUserRepository userRepository,
    IPasswordHashService passwordHashService,
    IAccessTokenService accessTokenService) : IRequestHandler<LoginCommand, AuthSessionResponse>
{
    public async Task<AuthSessionResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var normalizedEmail = User.NormalizeEmail(request.Email);
        var user = await userRepository.FindByNormalizedEmailAsync(normalizedEmail, cancellationToken);
        if (user is null || !passwordHashService.VerifyPassword(user.PasswordHash, request.Password))
        {
            throw new InvalidCredentialsException();
        }

        var accessToken = accessTokenService.CreateAccessToken(user);

        return new AuthSessionResponse(
            accessToken.Value,
            accessToken.ExpiresAt,
            new UserProfileDto(user.Id, user.Email, user.DisplayName));
    }
}
