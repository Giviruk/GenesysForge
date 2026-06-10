using GenesysForge.Application.Rules;
using GenesysForge.Application.Rules.GetRuleCatalog;
using GenesysForge.Application.Rules.GetRulesets;
using GenesysForge.Contracts.Rules;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GenesysForge.WebApi.Controllers;

[ApiController]
[AllowAnonymous]
[Route("rules")]
public sealed class RulesController(ISender sender) : ControllerBase
{
    [HttpGet("rulesets")]
    [ProducesResponseType<IReadOnlyCollection<RulesetDto>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyCollection<RulesetDto>>> GetRulesets(CancellationToken cancellationToken)
    {
        var response = await sender.Send(new GetRulesetsQuery(), cancellationToken);

        return Ok(response);
    }

    [HttpGet("{rulesetId:guid}/catalog")]
    [ProducesResponseType<RuleCatalogResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RuleCatalogResponse>> GetRuleCatalog(
        Guid rulesetId,
        CancellationToken cancellationToken)
    {
        try
        {
            var response = await sender.Send(new GetRuleCatalogQuery(rulesetId), cancellationToken);

            return Ok(response);
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
    }
}
