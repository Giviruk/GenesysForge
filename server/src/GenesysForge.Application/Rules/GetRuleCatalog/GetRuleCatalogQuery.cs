using GenesysForge.Contracts.Rules;
using MediatR;

namespace GenesysForge.Application.Rules.GetRuleCatalog;

public sealed record GetRuleCatalogQuery(Guid RulesetId) : IRequest<RuleCatalogResponse>;
