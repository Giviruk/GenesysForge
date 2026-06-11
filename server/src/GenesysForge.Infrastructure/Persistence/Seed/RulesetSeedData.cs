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
            "career",
            "warrior",
            "Воин",
            "Карьера бойца, который делает ставку на физическую подготовку и боевые навыки.",
            WarriorCareerId),
        RuleEntity.Create(
            DemoRulesetId,
            "career",
            "scholar",
            "Ученый",
            "Карьера исследователя, медика или наставника, который решает задачи знаниями.",
            ScholarCareerId),
        RuleEntity.Create(
            DemoRulesetId,
            "skill",
            "athletics",
            "Атлетика",
            "Общие физические действия: бег, прыжки, лазание и силовые испытания.",
            AthleticsSkillId),
        RuleEntity.Create(
            DemoRulesetId,
            "skill",
            "coordination",
            "Координация",
            "Общие действия на точность, равновесие и ловкость.",
            CoordinationSkillId),
        RuleEntity.Create(
            DemoRulesetId,
            "skill",
            "perception",
            "Внимание",
            "Общее наблюдение, поиск деталей и чтение окружения.",
            PerceptionSkillId),
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
            "skill",
            "mechanics",
            "Механика",
            "Обслуживание, ремонт и понимание устройств.",
            MechanicsSkillId),
        RuleEntity.Create(
            DemoRulesetId,
            "skill",
            "medicine",
            "Медицина",
            "Первая помощь, лечение ран и знание тела.",
            MedicineSkillId),
        RuleEntity.Create(
            DemoRulesetId,
            "skill",
            "charm",
            "Обаяние",
            "Социальное влияние через симпатию, доверие и открытость.",
            CharmSkillId),
        RuleEntity.Create(
            DemoRulesetId,
            "skill",
            "coercion",
            "Принуждение",
            "Социальное давление через угрозы, страх и силу позиции.",
            CoercionSkillId),
        RuleEntity.Create(
            DemoRulesetId,
            "skill",
            "deception",
            "Обман",
            "Социальная ложь, маскировка намерений и введение в заблуждение.",
            DeceptionSkillId),
        RuleEntity.Create(
            DemoRulesetId,
            "skill",
            "leadership",
            "Лидерство",
            "Социальное управление группой, вдохновение и командование.",
            LeadershipSkillId),
        RuleEntity.Create(
            DemoRulesetId,
            "skill",
            "negotiation",
            "Переговоры",
            "Социальный торг, сделки и поиск взаимной выгоды.",
            NegotiationSkillId),
        RuleEntity.Create(
            DemoRulesetId,
            "skill",
            "brawl",
            "Драка",
            "Бой без оружия и грубые физические столкновения.",
            BrawlSkillId),
        RuleEntity.Create(
            DemoRulesetId,
            "skill",
            "melee",
            "Холодное оружие",
            "Ближний бой с оружием.",
            MeleeSkillId),
        RuleEntity.Create(
            DemoRulesetId,
            "skill",
            "ranged",
            "Стрельба",
            "Дистанционные атаки легким оружием.",
            RangedSkillId),
        RuleEntity.Create(
            DemoRulesetId,
            "skill",
            "heavy-ranged",
            "Тяжелое оружие",
            "Дистанционные атаки тяжелым или громоздким оружием.",
            HeavyRangedSkillId),
        RuleEntity.Create(
            DemoRulesetId,
            "skill",
            "education",
            "Образование",
            "Знания академического, исторического и технического характера.",
            EducationSkillId),
        RuleEntity.Create(
            DemoRulesetId,
            "skill",
            "lore",
            "Лор",
            "Знания о легендах, традициях и необычных явлениях.",
            LoreSkillId),
        RuleEntity.Create(
            DemoRulesetId,
            "skill",
            "nature",
            "Природа",
            "Знания о дикой местности, существах и природных угрозах.",
            NatureSkillId),
        RuleEntity.Create(
            DemoRulesetId,
            "skill",
            "forbidden-knowledge",
            "Запретные знания",
            "Знания о тайных практиках, опасных теориях и скрытых культах.",
            ForbiddenKnowledgeSkillId),
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
            {"characteristics":{"brawn":3,"agility":2,"intellect":2,"cunning":2,"willpower":3,"presence":2},"derivedStats":{"woundThreshold":13,"strainThreshold":11,"soak":3,"meleeDefense":0,"rangedDefense":0}}
            """,
            GuardianDefinitionId),
        RuleDefinition.Create(
            MysticArchetypeId,
            DemoSourceVersionId,
            "starting-profile",
            """
            {"characteristics":{"brawn":2,"agility":2,"intellect":3,"cunning":2,"willpower":3,"presence":2},"derivedStats":{"woundThreshold":10,"strainThreshold":13,"soak":2,"meleeDefense":0,"rangedDefense":0}}
            """,
            MysticDefinitionId),
        RuleDefinition.Create(
            WarriorCareerId,
            DemoSourceVersionId,
            "career-profile",
            """
            {"careerSkillKeys":["athletics","resolve","brawl","melee","ranged","coercion","leadership","tactics"],"careerSkillRanksToAssign":4}
            """,
            WarriorDefinitionId),
        RuleDefinition.Create(
            ScholarCareerId,
            DemoSourceVersionId,
            "career-profile",
            """
            {"careerSkillKeys":["education","lore","medicine","mechanics","perception","negotiation","leadership","tactics"],"careerSkillRanksToAssign":4}
            """,
            ScholarDefinitionId),
        RuleDefinition.Create(
            AthleticsSkillId,
            DemoSourceVersionId,
            "skill-profile",
            """
            {"category":"general","characteristic":"brawn","cost":{"type":"rank-table","career":{"1":5,"2":10,"3":15,"4":20,"5":25},"nonCareer":{"1":10,"2":15,"3":20,"4":25,"5":30}}}
            """,
            AthleticsDefinitionId),
        RuleDefinition.Create(
            CoordinationSkillId,
            DemoSourceVersionId,
            "skill-profile",
            """
            {"category":"general","characteristic":"agility","cost":{"type":"rank-table","career":{"1":5,"2":10,"3":15,"4":20,"5":25},"nonCareer":{"1":10,"2":15,"3":20,"4":25,"5":30}}}
            """,
            CoordinationDefinitionId),
        RuleDefinition.Create(
            PerceptionSkillId,
            DemoSourceVersionId,
            "skill-profile",
            """
            {"category":"general","characteristic":"willpower","cost":{"type":"rank-table","career":{"1":5,"2":10,"3":15,"4":20,"5":25},"nonCareer":{"1":10,"2":15,"3":20,"4":25,"5":30}}}
            """,
            PerceptionDefinitionId),
        RuleDefinition.Create(
            ResolveSkillId,
            DemoSourceVersionId,
            "skill-profile",
            """
            {"category":"general","characteristic":"willpower","cost":{"type":"rank-table","career":{"1":5,"2":10,"3":15,"4":20,"5":25},"nonCareer":{"1":10,"2":15,"3":20,"4":25,"5":30}}}
            """,
            ResolveDefinitionId),
        RuleDefinition.Create(
            TacticsSkillId,
            DemoSourceVersionId,
            "skill-profile",
            """
            {"category":"general","characteristic":"intellect","cost":{"type":"rank-table","career":{"1":5,"2":10,"3":15,"4":20,"5":25},"nonCareer":{"1":10,"2":15,"3":20,"4":25,"5":30}}}
            """,
            TacticsDefinitionId),
        RuleDefinition.Create(
            MechanicsSkillId,
            DemoSourceVersionId,
            "skill-profile",
            """
            {"category":"general","characteristic":"intellect","cost":{"type":"rank-table","career":{"1":5,"2":10,"3":15,"4":20,"5":25},"nonCareer":{"1":10,"2":15,"3":20,"4":25,"5":30}}}
            """,
            MechanicsDefinitionId),
        RuleDefinition.Create(
            MedicineSkillId,
            DemoSourceVersionId,
            "skill-profile",
            """
            {"category":"general","characteristic":"intellect","cost":{"type":"rank-table","career":{"1":5,"2":10,"3":15,"4":20,"5":25},"nonCareer":{"1":10,"2":15,"3":20,"4":25,"5":30}}}
            """,
            MedicineDefinitionId),
        RuleDefinition.Create(
            CharmSkillId,
            DemoSourceVersionId,
            "skill-profile",
            """
            {"category":"social","characteristic":"presence","cost":{"type":"rank-table","career":{"1":5,"2":10,"3":15,"4":20,"5":25},"nonCareer":{"1":10,"2":15,"3":20,"4":25,"5":30}}}
            """,
            CharmDefinitionId),
        RuleDefinition.Create(
            CoercionSkillId,
            DemoSourceVersionId,
            "skill-profile",
            """
            {"category":"social","characteristic":"willpower","cost":{"type":"rank-table","career":{"1":5,"2":10,"3":15,"4":20,"5":25},"nonCareer":{"1":10,"2":15,"3":20,"4":25,"5":30}}}
            """,
            CoercionDefinitionId),
        RuleDefinition.Create(
            DeceptionSkillId,
            DemoSourceVersionId,
            "skill-profile",
            """
            {"category":"social","characteristic":"cunning","cost":{"type":"rank-table","career":{"1":5,"2":10,"3":15,"4":20,"5":25},"nonCareer":{"1":10,"2":15,"3":20,"4":25,"5":30}}}
            """,
            DeceptionDefinitionId),
        RuleDefinition.Create(
            LeadershipSkillId,
            DemoSourceVersionId,
            "skill-profile",
            """
            {"category":"social","characteristic":"presence","cost":{"type":"rank-table","career":{"1":5,"2":10,"3":15,"4":20,"5":25},"nonCareer":{"1":10,"2":15,"3":20,"4":25,"5":30}}}
            """,
            LeadershipDefinitionId),
        RuleDefinition.Create(
            NegotiationSkillId,
            DemoSourceVersionId,
            "skill-profile",
            """
            {"category":"social","characteristic":"presence","cost":{"type":"rank-table","career":{"1":5,"2":10,"3":15,"4":20,"5":25},"nonCareer":{"1":10,"2":15,"3":20,"4":25,"5":30}}}
            """,
            NegotiationDefinitionId),
        RuleDefinition.Create(
            BrawlSkillId,
            DemoSourceVersionId,
            "skill-profile",
            """
            {"category":"combat","characteristic":"brawn","cost":{"type":"rank-table","career":{"1":5,"2":10,"3":15,"4":20,"5":25},"nonCareer":{"1":10,"2":15,"3":20,"4":25,"5":30}}}
            """,
            BrawlDefinitionId),
        RuleDefinition.Create(
            MeleeSkillId,
            DemoSourceVersionId,
            "skill-profile",
            """
            {"category":"combat","characteristic":"brawn","cost":{"type":"rank-table","career":{"1":5,"2":10,"3":15,"4":20,"5":25},"nonCareer":{"1":10,"2":15,"3":20,"4":25,"5":30}}}
            """,
            MeleeDefinitionId),
        RuleDefinition.Create(
            RangedSkillId,
            DemoSourceVersionId,
            "skill-profile",
            """
            {"category":"combat","characteristic":"agility","cost":{"type":"rank-table","career":{"1":5,"2":10,"3":15,"4":20,"5":25},"nonCareer":{"1":10,"2":15,"3":20,"4":25,"5":30}}}
            """,
            RangedDefinitionId),
        RuleDefinition.Create(
            HeavyRangedSkillId,
            DemoSourceVersionId,
            "skill-profile",
            """
            {"category":"combat","characteristic":"agility","cost":{"type":"rank-table","career":{"1":5,"2":10,"3":15,"4":20,"5":25},"nonCareer":{"1":10,"2":15,"3":20,"4":25,"5":30}}}
            """,
            HeavyRangedDefinitionId),
        RuleDefinition.Create(
            EducationSkillId,
            DemoSourceVersionId,
            "skill-profile",
            """
            {"category":"knowledge","characteristic":"intellect","cost":{"type":"rank-table","career":{"1":5,"2":10,"3":15,"4":20,"5":25},"nonCareer":{"1":10,"2":15,"3":20,"4":25,"5":30}}}
            """,
            EducationDefinitionId),
        RuleDefinition.Create(
            LoreSkillId,
            DemoSourceVersionId,
            "skill-profile",
            """
            {"category":"knowledge","characteristic":"intellect","cost":{"type":"rank-table","career":{"1":5,"2":10,"3":15,"4":20,"5":25},"nonCareer":{"1":10,"2":15,"3":20,"4":25,"5":30}}}
            """,
            LoreDefinitionId),
        RuleDefinition.Create(
            NatureSkillId,
            DemoSourceVersionId,
            "skill-profile",
            """
            {"category":"knowledge","characteristic":"intellect","cost":{"type":"rank-table","career":{"1":5,"2":10,"3":15,"4":20,"5":25},"nonCareer":{"1":10,"2":15,"3":20,"4":25,"5":30}}}
            """,
            NatureDefinitionId),
        RuleDefinition.Create(
            ForbiddenKnowledgeSkillId,
            DemoSourceVersionId,
            "skill-profile",
            """
            {"category":"knowledge","characteristic":"intellect","cost":{"type":"rank-table","career":{"1":5,"2":10,"3":15,"4":20,"5":25},"nonCareer":{"1":10,"2":15,"3":20,"4":25,"5":30}}}
            """,
            ForbiddenKnowledgeDefinitionId),
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
