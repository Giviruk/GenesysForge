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
            TacticsSkillId),
        RuleEntity.Create(
            DemoRulesetId,
            "talent",
            "steady-stance",
            "Устойчивая стойка",
            "Демо-талант с фиксированной стоимостью для проверки покупки талантов.",
            SteadyStanceTalentId)
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
            {"characteristic":"willpower","isCareerDefault":false,"cost":{"type":"rank-table","career":{"1":5,"2":10,"3":15,"4":20,"5":25},"nonCareer":{"1":10,"2":15,"3":20,"4":25,"5":30}}}
            """,
            ResolveDefinitionId),
        RuleDefinition.Create(
            TacticsSkillId,
            DemoSourceVersionId,
            "skill-profile",
            """
            {"characteristic":"intellect","isCareerDefault":false,"cost":{"type":"rank-table","career":{"1":5,"2":10,"3":15,"4":20,"5":25},"nonCareer":{"1":10,"2":15,"3":20,"4":25,"5":30}}}
            """,
            TacticsDefinitionId),
        RuleDefinition.Create(
            SteadyStanceTalentId,
            DemoSourceVersionId,
            "talent-profile",
            """
            {"tier":1,"activation":"passive","requirements":[],"cost":{"type":"fixed","xp":5}}
            """,
            SteadyStanceDefinitionId)
    ];
}
