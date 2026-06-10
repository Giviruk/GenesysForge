using GenesysForge.Contracts.Rules;
using MediatR;

namespace GenesysForge.Application.Rules.GetRuleCatalog;

public sealed class GetRuleCatalogQueryHandler(IRulesRepository rulesRepository)
    : IRequestHandler<GetRuleCatalogQuery, RuleCatalogResponse>
{
    public async Task<RuleCatalogResponse> Handle(
        GetRuleCatalogQuery request,
        CancellationToken cancellationToken)
    {
        return await rulesRepository.GetCatalogAsync(request.RulesetId, cancellationToken)
            ?? throw new RulesetNotFoundException(request.RulesetId);
    }
}
