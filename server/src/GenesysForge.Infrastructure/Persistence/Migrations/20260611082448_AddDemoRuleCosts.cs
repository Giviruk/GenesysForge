using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GenesysForge.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddDemoRuleCosts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.InsertData(
                table: "RuleEntities",
                columns: new[] { "Id", "Description", "EntityType", "Key", "Name", "RulesetId" },
                values: new object[] { new Guid("10000000-0000-0000-0000-00000000000c"), "Демо-талант с фиксированной стоимостью для проверки покупки талантов.", "talent", "steady-stance", "Устойчивая стойка", new Guid("10000000-0000-0000-0000-000000000001") });

            migrationBuilder.InsertData(
                table: "RuleDefinitions",
                columns: new[] { "Id", "ContentJson", "Key", "RuleEntityId", "SourceVersionId" },
                values: new object[] { new Guid("10000000-0000-0000-0000-00000000000d"), "{\"tier\":1,\"activation\":\"passive\",\"requirements\":[],\"cost\":{\"type\":\"fixed\",\"xp\":5}}", "talent-profile", new Guid("10000000-0000-0000-0000-00000000000c"), new Guid("10000000-0000-0000-0000-000000000003") });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RuleDefinitions",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-00000000000d"));

            migrationBuilder.DeleteData(
                table: "RuleEntities",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-00000000000c"));

            migrationBuilder.UpdateData(
                table: "RuleDefinitions",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-00000000000a"),
                column: "ContentJson",
                value: "{\"characteristic\":\"willpower\",\"isCareerDefault\":false}");

            migrationBuilder.UpdateData(
                table: "RuleDefinitions",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-00000000000b"),
                column: "ContentJson",
                value: "{\"characteristic\":\"intellect\",\"isCareerDefault\":false}");
        }
    }
}
