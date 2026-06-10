using FluentValidation;
using GenesysForge.Application.Auth;
using GenesysForge.Application.Auth.Login;
using GenesysForge.Application.Auth.Register;
using GenesysForge.Contracts.Auth;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GenesysForge.WebApi.Controllers;

[ApiController]
[AllowAnonymous]
[Route("auth")]
public sealed class AuthController(ISender sender) : ControllerBase
{
    [HttpPost("register")]
    [ProducesResponseType<RegisterResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<RegisterResponse>> Register(
        RegisterRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var response = await sender.Send(
                new RegisterCommand(request.Email, request.Password, request.DisplayName),
                cancellationToken);

            return Ok(response);
        }
        catch (ValidationException exception)
        {
            return BadRequest(CreateValidationProblemDetails(exception));
        }
        catch (EmailAlreadyRegisteredException exception)
        {
            return Conflict(new ProblemDetails
            {
                Title = "Email already registered.",
                Detail = exception.Message,
                Status = StatusCodes.Status409Conflict
            });
        }
    }

    [HttpPost("login")]
    [ProducesResponseType<AuthSessionResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<AuthSessionResponse>> Login(
        LoginRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var response = await sender.Send(
                new LoginCommand(request.Email, request.Password),
                cancellationToken);

            return Ok(response);
        }
        catch (ValidationException exception)
        {
            return BadRequest(CreateValidationProblemDetails(exception));
        }
        catch (InvalidCredentialsException exception)
        {
            return Unauthorized(new ProblemDetails
            {
                Title = "Invalid credentials.",
                Detail = exception.Message,
                Status = StatusCodes.Status401Unauthorized
            });
        }
    }

    private static ValidationProblemDetails CreateValidationProblemDetails(ValidationException exception)
    {
        return new ValidationProblemDetails(ToErrorDictionary(exception))
        {
            Title = "Validation failed.",
            Status = StatusCodes.Status400BadRequest
        };
    }

    private static Dictionary<string, string[]> ToErrorDictionary(ValidationException exception)
    {
        return exception.Errors
            .GroupBy(error => error.PropertyName)
            .ToDictionary(
                group => group.Key,
                group => group.Select(error => error.ErrorMessage).ToArray());
    }
}
