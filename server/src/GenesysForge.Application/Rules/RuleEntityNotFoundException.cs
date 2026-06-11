namespace GenesysForge.Application.Rules;

public sealed class RuleEntityNotFoundException(Guid ruleEntityId, string entityType)
    : InvalidOperationException($"Rule entity '{ruleEntityId}' with type '{entityType}' was not found.");
