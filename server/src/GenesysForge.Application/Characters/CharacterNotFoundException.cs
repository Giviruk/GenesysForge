namespace GenesysForge.Application.Characters;

public sealed class CharacterNotFoundException(Guid characterId) : Exception($"Character '{characterId}' was not found.")
{
    public Guid CharacterId { get; } = characterId;
}
