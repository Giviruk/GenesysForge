using GenesysForge.Contracts.Rules;
using MediatR;

namespace GenesysForge.Application.Rules.GetRulesets;

public sealed record GetRulesetsQuery : IRequest<IReadOnlyCollection<RulesetDto>>;
