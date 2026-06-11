using GenesysForge.Contracts.Characters;
using GenesysForge.Contracts.Rules;
using Mapster;
using DomainCharacter = GenesysForge.Domain.Characters.Character;

namespace GenesysForge.Application.Characters;

internal static class CharacterResponseMapper
{
    private static readonly TypeAdapterConfig Config = CreateConfig();

    public static CharacterSummaryResponse ToSummaryResponse(DomainCharacter character)
    {
        return character.Adapt<CharacterSummaryResponse>(Config);
    }

    public static CharacterDetailResponse ToDetailResponse(DomainCharacter character)
    {
        return character.Adapt<CharacterDetailResponse>(Config);
    }

    private static TypeAdapterConfig CreateConfig()
    {
        var config = new TypeAdapterConfig();

        config.NewConfig<DomainCharacter, CharacterSummaryResponse>()
            .Map(destination => destination.UpdatedAt, source => source.UpdatedAt ?? source.CreatedAt);

        config.NewConfig<DomainCharacter, CharacterDetailResponse>()
            .Map(destination => destination.UpdatedAt, source => source.UpdatedAt ?? source.CreatedAt)
            .Map(destination => destination.RuleSnapshot, _ => (RuleSnapshotDto?)null)
            .Map(destination => destination.CalculatedStats, _ => (CalculatedCharacterStatsDto?)null);

        return config;
    }
}
