using GenesysForge.Contracts.Auth;
using MediatR;

namespace GenesysForge.Application.Auth.Register;

public sealed record RegisterCommand(
    string Email,
    string Password,
    string DisplayName) : IRequest<RegisterResponse>;
