namespace GenesysForge.Application.Rules;

public sealed class RulesetNotFoundException(Guid rulesetId)
    : InvalidOperationException($"Ruleset '{rulesetId}' was not found.");
