using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GenesysForge.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ExpandDemoRuleCatalog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "RuleDefinitions",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000008"),
                column: "ContentJson",
                value: "{\"characteristics\":{\"brawn\":3,\"agility\":2,\"intellect\":2,\"cunning\":2,\"willpower\":3,\"presence\":2},\"derivedStats\":{\"woundThreshold\":13,\"strainThreshold\":11,\"soak\":3,\"meleeDefense\":0,\"rangedDefense\":0}}");

            migrationBuilder.UpdateData(
                table: "RuleDefinitions",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000009"),
                column: "ContentJson",
                value: "{\"characteristics\":{\"brawn\":2,\"agility\":2,\"intellect\":3,\"cunning\":2,\"willpower\":3,\"presence\":2},\"derivedStats\":{\"woundThreshold\":10,\"strainThreshold\":13,\"soak\":2,\"meleeDefense\":0,\"rangedDefense\":0}}");

            migrationBuilder.UpdateData(
                table: "RuleDefinitions",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-00000000000a"),
                column: "ContentJson",
                value: "{\"category\":\"general\",\"characteristic\":\"willpower\",\"cost\":{\"type\":\"rank-table\",\"career\":{\"1\":5,\"2\":10,\"3\":15,\"4\":20,\"5\":25},\"nonCareer\":{\"1\":10,\"2\":15,\"3\":20,\"4\":25,\"5\":30}}}");

            migrationBuilder.UpdateData(
                table: "RuleDefinitions",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-00000000000b"),
                column: "ContentJson",
                value: "{\"category\":\"general\",\"characteristic\":\"intellect\",\"cost\":{\"type\":\"rank-table\",\"career\":{\"1\":5,\"2\":10,\"3\":15,\"4\":20,\"5\":25},\"nonCareer\":{\"1\":10,\"2\":15,\"3\":20,\"4\":25,\"5\":30}}}");

            migrationBuilder.InsertData(
                table: "RuleEntities",
                columns: new[] { "Id", "Description", "EntityType", "Key", "Name", "RulesetId" },
                values: new object[,]
                {
                    { new Guid("10000000-0000-0000-0000-00000000000e"), "Карьера бойца, который делает ставку на физическую подготовку и боевые навыки.", "career", "warrior", "Воин", new Guid("10000000-0000-0000-0000-000000000001") },
                    { new Guid("10000000-0000-0000-0000-00000000000f"), "Карьера исследователя, медика или наставника, который решает задачи знаниями.", "career", "scholar", "Ученый", new Guid("10000000-0000-0000-0000-000000000001") },
                    { new Guid("10000000-0000-0000-0000-000000000010"), "Общие физические действия: бег, прыжки, лазание и силовые испытания.", "skill", "athletics", "Атлетика", new Guid("10000000-0000-0000-0000-000000000001") },
                    { new Guid("10000000-0000-0000-0000-000000000011"), "Общие действия на точность, равновесие и ловкость.", "skill", "coordination", "Координация", new Guid("10000000-0000-0000-0000-000000000001") },
                    { new Guid("10000000-0000-0000-0000-000000000012"), "Общее наблюдение, поиск деталей и чтение окружения.", "skill", "perception", "Внимание", new Guid("10000000-0000-0000-0000-000000000001") },
                    { new Guid("10000000-0000-0000-0000-000000000013"), "Обслуживание, ремонт и понимание устройств.", "skill", "mechanics", "Механика", new Guid("10000000-0000-0000-0000-000000000001") },
                    { new Guid("10000000-0000-0000-0000-000000000014"), "Первая помощь, лечение ран и знание тела.", "skill", "medicine", "Медицина", new Guid("10000000-0000-0000-0000-000000000001") },
                    { new Guid("10000000-0000-0000-0000-000000000015"), "Социальное влияние через симпатию, доверие и открытость.", "skill", "charm", "Обаяние", new Guid("10000000-0000-0000-0000-000000000001") },
                    { new Guid("10000000-0000-0000-0000-000000000016"), "Социальное давление через угрозы, страх и силу позиции.", "skill", "coercion", "Принуждение", new Guid("10000000-0000-0000-0000-000000000001") },
                    { new Guid("10000000-0000-0000-0000-000000000017"), "Социальная ложь, маскировка намерений и введение в заблуждение.", "skill", "deception", "Обман", new Guid("10000000-0000-0000-0000-000000000001") },
                    { new Guid("10000000-0000-0000-0000-000000000018"), "Социальное управление группой, вдохновение и командование.", "skill", "leadership", "Лидерство", new Guid("10000000-0000-0000-0000-000000000001") },
                    { new Guid("10000000-0000-0000-0000-000000000019"), "Социальный торг, сделки и поиск взаимной выгоды.", "skill", "negotiation", "Переговоры", new Guid("10000000-0000-0000-0000-000000000001") },
                    { new Guid("10000000-0000-0000-0000-00000000001a"), "Бой без оружия и грубые физические столкновения.", "skill", "brawl", "Драка", new Guid("10000000-0000-0000-0000-000000000001") },
                    { new Guid("10000000-0000-0000-0000-00000000001b"), "Ближний бой с оружием.", "skill", "melee", "Холодное оружие", new Guid("10000000-0000-0000-0000-000000000001") },
                    { new Guid("10000000-0000-0000-0000-00000000001c"), "Дистанционные атаки легким оружием.", "skill", "ranged", "Стрельба", new Guid("10000000-0000-0000-0000-000000000001") },
                    { new Guid("10000000-0000-0000-0000-00000000001d"), "Дистанционные атаки тяжелым или громоздким оружием.", "skill", "heavy-ranged", "Тяжелое оружие", new Guid("10000000-0000-0000-0000-000000000001") },
                    { new Guid("10000000-0000-0000-0000-00000000001e"), "Знания академического, исторического и технического характера.", "skill", "education", "Образование", new Guid("10000000-0000-0000-0000-000000000001") },
                    { new Guid("10000000-0000-0000-0000-00000000001f"), "Знания о легендах, традициях и необычных явлениях.", "skill", "lore", "Лор", new Guid("10000000-0000-0000-0000-000000000001") },
                    { new Guid("10000000-0000-0000-0000-000000000020"), "Знания о дикой местности, существах и природных угрозах.", "skill", "nature", "Природа", new Guid("10000000-0000-0000-0000-000000000001") },
                    { new Guid("10000000-0000-0000-0000-000000000021"), "Знания о тайных практиках, опасных теориях и скрытых культах.", "skill", "forbidden-knowledge", "Запретные знания", new Guid("10000000-0000-0000-0000-000000000001") }
                });

            migrationBuilder.InsertData(
                table: "RuleDefinitions",
                columns: new[] { "Id", "ContentJson", "Key", "RuleEntityId", "SourceVersionId" },
                values: new object[,]
                {
                    { new Guid("10000000-0000-0000-0000-000000000022"), "{\"careerSkillKeys\":[\"athletics\",\"resolve\",\"brawl\",\"melee\",\"ranged\",\"coercion\",\"leadership\",\"tactics\"],\"careerSkillRanksToAssign\":4}", "career-profile", new Guid("10000000-0000-0000-0000-00000000000e"), new Guid("10000000-0000-0000-0000-000000000003") },
                    { new Guid("10000000-0000-0000-0000-000000000023"), "{\"careerSkillKeys\":[\"education\",\"lore\",\"medicine\",\"mechanics\",\"perception\",\"negotiation\",\"leadership\",\"tactics\"],\"careerSkillRanksToAssign\":4}", "career-profile", new Guid("10000000-0000-0000-0000-00000000000f"), new Guid("10000000-0000-0000-0000-000000000003") },
                    { new Guid("10000000-0000-0000-0000-000000000024"), "{\"category\":\"general\",\"characteristic\":\"brawn\",\"cost\":{\"type\":\"rank-table\",\"career\":{\"1\":5,\"2\":10,\"3\":15,\"4\":20,\"5\":25},\"nonCareer\":{\"1\":10,\"2\":15,\"3\":20,\"4\":25,\"5\":30}}}", "skill-profile", new Guid("10000000-0000-0000-0000-000000000010"), new Guid("10000000-0000-0000-0000-000000000003") },
                    { new Guid("10000000-0000-0000-0000-000000000025"), "{\"category\":\"general\",\"characteristic\":\"agility\",\"cost\":{\"type\":\"rank-table\",\"career\":{\"1\":5,\"2\":10,\"3\":15,\"4\":20,\"5\":25},\"nonCareer\":{\"1\":10,\"2\":15,\"3\":20,\"4\":25,\"5\":30}}}", "skill-profile", new Guid("10000000-0000-0000-0000-000000000011"), new Guid("10000000-0000-0000-0000-000000000003") },
                    { new Guid("10000000-0000-0000-0000-000000000026"), "{\"category\":\"general\",\"characteristic\":\"willpower\",\"cost\":{\"type\":\"rank-table\",\"career\":{\"1\":5,\"2\":10,\"3\":15,\"4\":20,\"5\":25},\"nonCareer\":{\"1\":10,\"2\":15,\"3\":20,\"4\":25,\"5\":30}}}", "skill-profile", new Guid("10000000-0000-0000-0000-000000000012"), new Guid("10000000-0000-0000-0000-000000000003") },
                    { new Guid("10000000-0000-0000-0000-000000000027"), "{\"category\":\"general\",\"characteristic\":\"intellect\",\"cost\":{\"type\":\"rank-table\",\"career\":{\"1\":5,\"2\":10,\"3\":15,\"4\":20,\"5\":25},\"nonCareer\":{\"1\":10,\"2\":15,\"3\":20,\"4\":25,\"5\":30}}}", "skill-profile", new Guid("10000000-0000-0000-0000-000000000013"), new Guid("10000000-0000-0000-0000-000000000003") },
                    { new Guid("10000000-0000-0000-0000-000000000028"), "{\"category\":\"general\",\"characteristic\":\"intellect\",\"cost\":{\"type\":\"rank-table\",\"career\":{\"1\":5,\"2\":10,\"3\":15,\"4\":20,\"5\":25},\"nonCareer\":{\"1\":10,\"2\":15,\"3\":20,\"4\":25,\"5\":30}}}", "skill-profile", new Guid("10000000-0000-0000-0000-000000000014"), new Guid("10000000-0000-0000-0000-000000000003") },
                    { new Guid("10000000-0000-0000-0000-000000000029"), "{\"category\":\"social\",\"characteristic\":\"presence\",\"cost\":{\"type\":\"rank-table\",\"career\":{\"1\":5,\"2\":10,\"3\":15,\"4\":20,\"5\":25},\"nonCareer\":{\"1\":10,\"2\":15,\"3\":20,\"4\":25,\"5\":30}}}", "skill-profile", new Guid("10000000-0000-0000-0000-000000000015"), new Guid("10000000-0000-0000-0000-000000000003") },
                    { new Guid("10000000-0000-0000-0000-00000000002a"), "{\"category\":\"social\",\"characteristic\":\"willpower\",\"cost\":{\"type\":\"rank-table\",\"career\":{\"1\":5,\"2\":10,\"3\":15,\"4\":20,\"5\":25},\"nonCareer\":{\"1\":10,\"2\":15,\"3\":20,\"4\":25,\"5\":30}}}", "skill-profile", new Guid("10000000-0000-0000-0000-000000000016"), new Guid("10000000-0000-0000-0000-000000000003") },
                    { new Guid("10000000-0000-0000-0000-00000000002b"), "{\"category\":\"social\",\"characteristic\":\"cunning\",\"cost\":{\"type\":\"rank-table\",\"career\":{\"1\":5,\"2\":10,\"3\":15,\"4\":20,\"5\":25},\"nonCareer\":{\"1\":10,\"2\":15,\"3\":20,\"4\":25,\"5\":30}}}", "skill-profile", new Guid("10000000-0000-0000-0000-000000000017"), new Guid("10000000-0000-0000-0000-000000000003") },
                    { new Guid("10000000-0000-0000-0000-00000000002c"), "{\"category\":\"social\",\"characteristic\":\"presence\",\"cost\":{\"type\":\"rank-table\",\"career\":{\"1\":5,\"2\":10,\"3\":15,\"4\":20,\"5\":25},\"nonCareer\":{\"1\":10,\"2\":15,\"3\":20,\"4\":25,\"5\":30}}}", "skill-profile", new Guid("10000000-0000-0000-0000-000000000018"), new Guid("10000000-0000-0000-0000-000000000003") },
                    { new Guid("10000000-0000-0000-0000-00000000002d"), "{\"category\":\"social\",\"characteristic\":\"presence\",\"cost\":{\"type\":\"rank-table\",\"career\":{\"1\":5,\"2\":10,\"3\":15,\"4\":20,\"5\":25},\"nonCareer\":{\"1\":10,\"2\":15,\"3\":20,\"4\":25,\"5\":30}}}", "skill-profile", new Guid("10000000-0000-0000-0000-000000000019"), new Guid("10000000-0000-0000-0000-000000000003") },
                    { new Guid("10000000-0000-0000-0000-00000000002e"), "{\"category\":\"combat\",\"characteristic\":\"brawn\",\"cost\":{\"type\":\"rank-table\",\"career\":{\"1\":5,\"2\":10,\"3\":15,\"4\":20,\"5\":25},\"nonCareer\":{\"1\":10,\"2\":15,\"3\":20,\"4\":25,\"5\":30}}}", "skill-profile", new Guid("10000000-0000-0000-0000-00000000001a"), new Guid("10000000-0000-0000-0000-000000000003") },
                    { new Guid("10000000-0000-0000-0000-00000000002f"), "{\"category\":\"combat\",\"characteristic\":\"brawn\",\"cost\":{\"type\":\"rank-table\",\"career\":{\"1\":5,\"2\":10,\"3\":15,\"4\":20,\"5\":25},\"nonCareer\":{\"1\":10,\"2\":15,\"3\":20,\"4\":25,\"5\":30}}}", "skill-profile", new Guid("10000000-0000-0000-0000-00000000001b"), new Guid("10000000-0000-0000-0000-000000000003") },
                    { new Guid("10000000-0000-0000-0000-000000000030"), "{\"category\":\"combat\",\"characteristic\":\"agility\",\"cost\":{\"type\":\"rank-table\",\"career\":{\"1\":5,\"2\":10,\"3\":15,\"4\":20,\"5\":25},\"nonCareer\":{\"1\":10,\"2\":15,\"3\":20,\"4\":25,\"5\":30}}}", "skill-profile", new Guid("10000000-0000-0000-0000-00000000001c"), new Guid("10000000-0000-0000-0000-000000000003") },
                    { new Guid("10000000-0000-0000-0000-000000000031"), "{\"category\":\"combat\",\"characteristic\":\"agility\",\"cost\":{\"type\":\"rank-table\",\"career\":{\"1\":5,\"2\":10,\"3\":15,\"4\":20,\"5\":25},\"nonCareer\":{\"1\":10,\"2\":15,\"3\":20,\"4\":25,\"5\":30}}}", "skill-profile", new Guid("10000000-0000-0000-0000-00000000001d"), new Guid("10000000-0000-0000-0000-000000000003") },
                    { new Guid("10000000-0000-0000-0000-000000000032"), "{\"category\":\"knowledge\",\"characteristic\":\"intellect\",\"cost\":{\"type\":\"rank-table\",\"career\":{\"1\":5,\"2\":10,\"3\":15,\"4\":20,\"5\":25},\"nonCareer\":{\"1\":10,\"2\":15,\"3\":20,\"4\":25,\"5\":30}}}", "skill-profile", new Guid("10000000-0000-0000-0000-00000000001e"), new Guid("10000000-0000-0000-0000-000000000003") },
                    { new Guid("10000000-0000-0000-0000-000000000033"), "{\"category\":\"knowledge\",\"characteristic\":\"intellect\",\"cost\":{\"type\":\"rank-table\",\"career\":{\"1\":5,\"2\":10,\"3\":15,\"4\":20,\"5\":25},\"nonCareer\":{\"1\":10,\"2\":15,\"3\":20,\"4\":25,\"5\":30}}}", "skill-profile", new Guid("10000000-0000-0000-0000-00000000001f"), new Guid("10000000-0000-0000-0000-000000000003") },
                    { new Guid("10000000-0000-0000-0000-000000000034"), "{\"category\":\"knowledge\",\"characteristic\":\"intellect\",\"cost\":{\"type\":\"rank-table\",\"career\":{\"1\":5,\"2\":10,\"3\":15,\"4\":20,\"5\":25},\"nonCareer\":{\"1\":10,\"2\":15,\"3\":20,\"4\":25,\"5\":30}}}", "skill-profile", new Guid("10000000-0000-0000-0000-000000000020"), new Guid("10000000-0000-0000-0000-000000000003") },
                    { new Guid("10000000-0000-0000-0000-000000000035"), "{\"category\":\"knowledge\",\"characteristic\":\"intellect\",\"cost\":{\"type\":\"rank-table\",\"career\":{\"1\":5,\"2\":10,\"3\":15,\"4\":20,\"5\":25},\"nonCareer\":{\"1\":10,\"2\":15,\"3\":20,\"4\":25,\"5\":30}}}", "skill-profile", new Guid("10000000-0000-0000-0000-000000000021"), new Guid("10000000-0000-0000-0000-000000000003") }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RuleDefinitions",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000022"));

            migrationBuilder.DeleteData(
                table: "RuleDefinitions",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000023"));

            migrationBuilder.DeleteData(
                table: "RuleDefinitions",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000024"));

            migrationBuilder.DeleteData(
                table: "RuleDefinitions",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000025"));

            migrationBuilder.DeleteData(
                table: "RuleDefinitions",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000026"));

            migrationBuilder.DeleteData(
                table: "RuleDefinitions",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000027"));

            migrationBuilder.DeleteData(
                table: "RuleDefinitions",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000028"));

            migrationBuilder.DeleteData(
                table: "RuleDefinitions",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000029"));

            migrationBuilder.DeleteData(
                table: "RuleDefinitions",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-00000000002a"));

            migrationBuilder.DeleteData(
                table: "RuleDefinitions",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-00000000002b"));

            migrationBuilder.DeleteData(
                table: "RuleDefinitions",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-00000000002c"));

            migrationBuilder.DeleteData(
                table: "RuleDefinitions",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-00000000002d"));

            migrationBuilder.DeleteData(
                table: "RuleDefinitions",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-00000000002e"));

            migrationBuilder.DeleteData(
                table: "RuleDefinitions",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-00000000002f"));

            migrationBuilder.DeleteData(
                table: "RuleDefinitions",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000030"));

            migrationBuilder.DeleteData(
                table: "RuleDefinitions",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000031"));

            migrationBuilder.DeleteData(
                table: "RuleDefinitions",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000032"));

            migrationBuilder.DeleteData(
                table: "RuleDefinitions",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000033"));

            migrationBuilder.DeleteData(
                table: "RuleDefinitions",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000034"));

            migrationBuilder.DeleteData(
                table: "RuleDefinitions",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000035"));

            migrationBuilder.DeleteData(
                table: "RuleEntities",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-00000000000e"));

            migrationBuilder.DeleteData(
                table: "RuleEntities",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-00000000000f"));

            migrationBuilder.DeleteData(
                table: "RuleEntities",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000010"));

            migrationBuilder.DeleteData(
                table: "RuleEntities",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000011"));

            migrationBuilder.DeleteData(
                table: "RuleEntities",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000012"));

            migrationBuilder.DeleteData(
                table: "RuleEntities",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000013"));

            migrationBuilder.DeleteData(
                table: "RuleEntities",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000014"));

            migrationBuilder.DeleteData(
                table: "RuleEntities",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000015"));

            migrationBuilder.DeleteData(
                table: "RuleEntities",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000016"));

            migrationBuilder.DeleteData(
                table: "RuleEntities",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000017"));

            migrationBuilder.DeleteData(
                table: "RuleEntities",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000018"));

            migrationBuilder.DeleteData(
                table: "RuleEntities",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000019"));

            migrationBuilder.DeleteData(
                table: "RuleEntities",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-00000000001a"));

            migrationBuilder.DeleteData(
                table: "RuleEntities",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-00000000001b"));

            migrationBuilder.DeleteData(
                table: "RuleEntities",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-00000000001c"));

            migrationBuilder.DeleteData(
                table: "RuleEntities",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-00000000001d"));

            migrationBuilder.DeleteData(
                table: "RuleEntities",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-00000000001e"));

            migrationBuilder.DeleteData(
                table: "RuleEntities",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-00000000001f"));

            migrationBuilder.DeleteData(
                table: "RuleEntities",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000020"));

            migrationBuilder.DeleteData(
                table: "RuleEntities",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000021"));

            migrationBuilder.UpdateData(
                table: "RuleDefinitions",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000008"),
                column: "ContentJson",
                value: "{\"woundThresholdBonus\":2,\"strainThresholdBonus\":0,\"startingSkillKeys\":[\"resolve\",\"tactics\"]}");

            migrationBuilder.UpdateData(
                table: "RuleDefinitions",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000009"),
                column: "ContentJson",
                value: "{\"woundThresholdBonus\":0,\"strainThresholdBonus\":2,\"startingSkillKeys\":[\"resolve\"]}");

            migrationBuilder.UpdateData(
                table: "RuleDefinitions",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-00000000000a"),
                column: "ContentJson",
                value: "{\"characteristic\":\"willpower\",\"isCareerDefault\":false,\"cost\":{\"type\":\"rank-table\",\"career\":{\"1\":5,\"2\":10,\"3\":15,\"4\":20,\"5\":25},\"nonCareer\":{\"1\":10,\"2\":15,\"3\":20,\"4\":25,\"5\":30}}}");

            migrationBuilder.UpdateData(
                table: "RuleDefinitions",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-00000000000b"),
                column: "ContentJson",
                value: "{\"characteristic\":\"intellect\",\"isCareerDefault\":false,\"cost\":{\"type\":\"rank-table\",\"career\":{\"1\":5,\"2\":10,\"3\":15,\"4\":20,\"5\":25},\"nonCareer\":{\"1\":10,\"2\":15,\"3\":20,\"4\":25,\"5\":30}}}");
        }
    }
}
