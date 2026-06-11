using System.Text.Json;
using GenesysForge.Application.Rules;
using GenesysForge.Contracts.Characters;
using GenesysForge.Domain.Characters;
using GenesysForge.Domain.Rules;
using MediatR;

namespace GenesysForge.Application.Characters.CreateCharacterDraft;

public sealed class CreateCharacterDraftCommandHandler(ICharactersRepository charactersRepository)
    : IRequestHandler<CreateCharacterDraftCommand, CharacterDetailResponse>
{
    private const string ArchetypeEntityType = "archetype";
    private const string CareerEntityType = "career";
    private const string SkillEntityType = "skill";
    private const string CareerProfileDefinitionKey = "career-profile";
    private const string StartingProfileDefinitionKey = "starting-profile";

    public async Task<CharacterDetailResponse> Handle(
        CreateCharacterDraftCommand request,
        CancellationToken cancellationToken)
    {
        var rulesetExists = await charactersRepository.RulesetExistsAsync(request.RulesetId, cancellationToken);
        if (!rulesetExists)
        {
            throw new RulesetNotFoundException(request.RulesetId);
        }

        var archetypes = await charactersRepository.ListRuleEntitiesAsync(
            request.RulesetId,
            ArchetypeEntityType,
            cancellationToken);
        var careers = await charactersRepository.ListRuleEntitiesAsync(
            request.RulesetId,
            CareerEntityType,
            cancellationToken);
        var skills = await charactersRepository.ListRuleEntitiesAsync(
            request.RulesetId,
            SkillEntityType,
            cancellationToken);

        var selectedArchetype = SelectRuleEntity(archetypes, request.ArchetypeId, ArchetypeEntityType);
        var selectedCareer = SelectRuleEntity(careers, request.CareerId, CareerEntityType);
        var careerProfile = await GetCareerProfileAsync(selectedCareer, cancellationToken);
        var archetypeProfile = await GetArchetypeProfileAsync(selectedArchetype, cancellationToken);

        var careerSkillKeys = careerProfile?.CareerSkillKeys.ToHashSet(StringComparer.OrdinalIgnoreCase) ?? [];
        var character = Character.CreateDraft(request.OwnerUserId, request.RulesetId, request.Name);
        var createdAt = DateTimeOffset.UtcNow;

        foreach (var skill in skills)
        {
            character.SetSkillRank(
                skill.Id,
                rank: 0,
                xpSpent: 0,
                isCareerSkill: careerSkillKeys.Contains(skill.Key),
                createdAt);
        }

        character.AddSnapshot(
            JsonSerializer.Serialize(new DraftProfileSnapshot(
                selectedArchetype?.Id,
                selectedCareer?.Id,
                careerProfile?.CareerSkillRanksToAssign ?? 0,
                archetypeProfile?.Characteristics ?? new Dictionary<string, int>(),
                archetypeProfile?.DerivedStats ?? new Dictionary<string, int>())),
            createdAt);

        await charactersRepository.AddAsync(character, cancellationToken);
        await charactersRepository.SaveChangesAsync(cancellationToken);

        return CharacterResponseMapper.ToDetailResponse(character);
    }

    private static RuleEntity? SelectRuleEntity(
        IReadOnlyCollection<RuleEntity> entities,
        Guid? requestedRuleEntityId,
        string entityType)
    {
        if (requestedRuleEntityId is null)
        {
            return entities.FirstOrDefault();
        }

        return entities.SingleOrDefault(entity => entity.Id == requestedRuleEntityId)
            ?? throw new RuleEntityNotFoundException(requestedRuleEntityId.Value, entityType);
    }

    private async Task<CareerProfile?> GetCareerProfileAsync(RuleEntity? career, CancellationToken cancellationToken)
    {
        if (career is null)
        {
            return null;
        }

        var definitions = await charactersRepository.GetRuleDefinitionContentByEntityIdsAsync(
            [career.Id],
            CareerProfileDefinitionKey,
            cancellationToken);

        return definitions.TryGetValue(career.Id, out var contentJson)
            ? JsonSerializer.Deserialize<CareerProfile>(contentJson, JsonOptions)
            : null;
    }

    private async Task<ArchetypeProfile?> GetArchetypeProfileAsync(RuleEntity? archetype, CancellationToken cancellationToken)
    {
        if (archetype is null)
        {
            return null;
        }

        var definitions = await charactersRepository.GetRuleDefinitionContentByEntityIdsAsync(
            [archetype.Id],
            StartingProfileDefinitionKey,
            cancellationToken);

        return definitions.TryGetValue(archetype.Id, out var contentJson)
            ? JsonSerializer.Deserialize<ArchetypeProfile>(contentJson, JsonOptions)
            : null;
    }

    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    private sealed record ArchetypeProfile(
        Dictionary<string, int> Characteristics,
        Dictionary<string, int> DerivedStats);

    private sealed record CareerProfile(
        IReadOnlyCollection<string> CareerSkillKeys,
        int CareerSkillRanksToAssign);

    private sealed record DraftProfileSnapshot(
        Guid? ArchetypeId,
        Guid? CareerId,
        int CareerSkillRanksToAssign,
        IReadOnlyDictionary<string, int> Characteristics,
        IReadOnlyDictionary<string, int> DerivedStats);
}
