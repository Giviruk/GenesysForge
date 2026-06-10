using GenesysForge.Contracts.Auth;
using GenesysForge.Domain.Users;
using MediatR;

namespace GenesysForge.Application.Auth.Register;

public sealed class RegisterCommandHandler(
    IUserRepository userRepository,
    IPasswordHashService passwordHashService) : IRequestHandler<RegisterCommand, RegisterResponse>
{
    public async Task<RegisterResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var normalizedEmail = User.NormalizeEmail(request.Email);
        var existingUser = await userRepository.FindByNormalizedEmailAsync(normalizedEmail, cancellationToken);
        if (existingUser is not null)
        {
            throw new EmailAlreadyRegisteredException(request.Email);
        }

        var passwordHash = passwordHashService.HashPassword(request.Password);
        var user = User.Create(request.Email, request.DisplayName, passwordHash);

        await userRepository.AddAsync(user, cancellationToken);
        await userRepository.SaveChangesAsync(cancellationToken);

        return new RegisterResponse(user.Id, user.Email, user.DisplayName);
    }
}
