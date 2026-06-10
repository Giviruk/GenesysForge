using GenesysForge.Contracts.Auth;
using MediatR;

namespace GenesysForge.Application.Auth.Login;

public sealed record LoginCommand(
    string Email,
    string Password) : IRequest<AuthSessionResponse>;
