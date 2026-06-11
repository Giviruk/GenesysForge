using System.Security.Claims;
using FluentValidation;
using GenesysForge.Application.Characters;
using GenesysForge.Application.Characters.CreateCharacterDraft;
using GenesysForge.Application.Characters.GetCharacterById;
using GenesysForge.Application.Characters.ListMyCharacters;
using GenesysForge.Application.Characters.ValidateCharacter;
using GenesysForge.Application.Rules;
using GenesysForge.Contracts.Characters;
using GenesysForge.Contracts.Validation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GenesysForge.WebApi.Controllers;

[ApiController]
[Authorize]
[Route("characters")]
public sealed class CharactersController(ISender sender) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType<CharacterDetailResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CharacterDetailResponse>> CreateDraft(
        CreateCharacterDraftRequest request,
        CancellationToken cancellationToken)
    {
        if (!TryGetCurrentUserId(out var ownerUserId))
        {
            return Unauthorized(CreateUnauthorizedProblemDetails());
        }

        try
        {
            var response = await sender.Send(
                new CreateCharacterDraftCommand(
                    ownerUserId,
                    request.RulesetId,
                    request.Name,
                    request.ArchetypeId,
                    request.CareerId),
                cancellationToken);

            return CreatedAtAction(nameof(GetById), new { characterId = response.Id }, response);
        }
        catch (ValidationException exception)
        {
            return BadRequest(CreateValidationProblemDetails(exception));
        }
        catch (RulesetNotFoundException exception)
        {
            return NotFound(new ProblemDetails
            {
                Title = "Набор правил не найден.",
                Detail = exception.Message,
                Status = StatusCodes.Status404NotFound
            });
        }
        catch (RuleEntityNotFoundException exception)
        {
            return NotFound(new ProblemDetails
            {
                Title = "Элемент правил не найден.",
                Detail = exception.Message,
                Status = StatusCodes.Status404NotFound
            });
        }
    }

    [HttpGet]
    [ProducesResponseType<IReadOnlyCollection<CharacterSummaryResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<IReadOnlyCollection<CharacterSummaryResponse>>> ListMine(CancellationToken cancellationToken)
    {
        if (!TryGetCurrentUserId(out var ownerUserId))
        {
            return Unauthorized(CreateUnauthorizedProblemDetails());
        }

        var response = await sender.Send(new ListMyCharactersQuery(ownerUserId), cancellationToken);

        return Ok(response);
    }

    [HttpGet("{characterId:guid}/validation")]
    [ProducesResponseType<ValidationResultResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ValidationResultResponse>> Validate(
        Guid characterId,
        CancellationToken cancellationToken)
    {
        if (!TryGetCurrentUserId(out var ownerUserId))
        {
            return Unauthorized(CreateUnauthorizedProblemDetails());
        }

        try
        {
            var response = await sender.Send(
                new ValidateCharacterQuery(ownerUserId, characterId),
                cancellationToken);

            return Ok(response);
        }
        catch (CharacterNotFoundException exception)
        {
            return NotFound(new ProblemDetails
            {
                Title = "Персонаж не найден.",
                Detail = exception.Message,
                Status = StatusCodes.Status404NotFound
            });
        }
    }

    [HttpGet("{characterId:guid}")]
    [ProducesResponseType<CharacterDetailResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CharacterDetailResponse>> GetById(
        Guid characterId,
        CancellationToken cancellationToken)
    {
        if (!TryGetCurrentUserId(out var ownerUserId))
        {
            return Unauthorized(CreateUnauthorizedProblemDetails());
        }

        try
        {
            var response = await sender.Send(
                new GetCharacterByIdQuery(ownerUserId, characterId),
                cancellationToken);

            return Ok(response);
        }
        catch (CharacterNotFoundException exception)
        {
            return NotFound(new ProblemDetails
            {
                Title = "Персонаж не найден.",
                Detail = exception.Message,
                Status = StatusCodes.Status404NotFound
            });
        }
    }

    private bool TryGetCurrentUserId(out Guid userId)
    {
        var value = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.TryParse(value, out userId);
    }

    private static ProblemDetails CreateUnauthorizedProblemDetails()
    {
        return new ProblemDetails
        {
            Title = "Пользователь не определен.",
            Status = StatusCodes.Status401Unauthorized
        };
    }

    private static ValidationProblemDetails CreateValidationProblemDetails(ValidationException exception)
    {
        return new ValidationProblemDetails(ToErrorDictionary(exception))
        {
            Title = "Ошибка валидации.",
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
