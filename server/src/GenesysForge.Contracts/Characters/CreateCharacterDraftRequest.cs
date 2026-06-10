namespace GenesysForge.Contracts.Characters;

public sealed record CreateCharacterDraftRequest(
    string Name,
    Guid RulesetId);
