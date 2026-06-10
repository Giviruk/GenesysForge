using GenesysForge.Contracts.Rules;
using MediatR;

namespace GenesysForge.Application.Rules.GetRulesets;

public sealed class GetRulesetsQueryHandler(IRulesRepository rulesRepository)
    : IRequestHandler<GetRulesetsQuery, IReadOnlyCollection<RulesetDto>>
{
    public Task<IReadOnlyCollection<RulesetDto>> Handle(
        GetRulesetsQuery request,
        CancellationToken cancellationToken)
    {
        return rulesRepository.ListRulesetsAsync(cancellationToken);
    }
}
