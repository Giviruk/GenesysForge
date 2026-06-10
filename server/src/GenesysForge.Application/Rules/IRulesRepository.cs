using GenesysForge.Contracts.Rules;

namespace GenesysForge.Application.Rules;

public interface IRulesRepository
{
    Task<RuleCatalogResponse?> GetCatalogAsync(Guid rulesetId, CancellationToken cancellationToken);

    Task<IReadOnlyCollection<RulesetDto>> ListRulesetsAsync(CancellationToken cancellationToken);
}
