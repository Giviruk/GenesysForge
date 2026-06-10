using GenesysForge.Domain.Rules;
using static GenesysForge.Infrastructure.Persistence.Seed.RulesetSeedIds;

namespace GenesysForge.Infrastructure.Persistence.Seed;

internal static class RulesetSeedData
{
    public static Ruleset Ruleset => Ruleset.Create(
        "Демо-набор Genesys Forge",
        "1.0",
        "Открытый демо-набор для проверки rules-driven данных без защищенного контента.",
        DemoRulesetId);

    public static SourceBook SourceBook => SourceBook.Create(
        DemoRulesetId,
        "demo-core",
        "Демо-правила",
        DemoSourceBookId);

    public static RuleSourceVersion SourceVersion => RuleSourceVersion.Create(
        DemoSourceBookId,
        "1.0",
        isActive: true,
        DemoSourceVersionId);

    public static IReadOnlyCollection<RuleEntity> Entities =>
    [
        RuleEntity.Create(
            DemoRulesetId,
            "archetype",
            "guardian",
            "Страж",
            "Защитник, который держит строй и помогает группе пережить опасность.",
            GuardianArchetypeId),
        RuleEntity.Create(
            DemoRulesetId,
            "archetype",
            "mystic",
            "Мистик",
            "Исследователь необычного, полагающийся на интуицию и внутреннюю дисциплину.",
            MysticArchetypeId),
        RuleEntity.Create(
            DemoRulesetId,
            "skill",
            "resolve",
            "Стойкость",
            "Навык спокойствия, выдержки и сопротивления давлению.",
            ResolveSkillId),
        RuleEntity.Create(
            DemoRulesetId,
            "skill",
            "tactics",
            "Тактика",
            "Навык планирования действий и чтения ситуации.",
            TacticsSkillId)
    ];

    public static IReadOnlyCollection<RuleDefinition> Definitions =>
    [
        RuleDefinition.Create(
            GuardianArchetypeId,
            DemoSourceVersionId,
            "starting-profile",
            """
            {"woundThresholdBonus":2,"strainThresholdBonus":0,"startingSkillKeys":["resolve","tactics"]}
            """,
            GuardianDefinitionId),
        RuleDefinition.Create(
            MysticArchetypeId,
            DemoSourceVersionId,
            "starting-profile",
            """
            {"woundThresholdBonus":0,"strainThresholdBonus":2,"startingSkillKeys":["resolve"]}
            """,
            MysticDefinitionId),
        RuleDefinition.Create(
            ResolveSkillId,
            DemoSourceVersionId,
            "skill-profile",
            """
            {"characteristic":"willpower","isCareerDefault":false}
            """,
            ResolveDefinitionId),
        RuleDefinition.Create(
            TacticsSkillId,
            DemoSourceVersionId,
            "skill-profile",
            """
            {"characteristic":"intellect","isCareerDefault":false}
            """,
            TacticsDefinitionId)
    ];
}
