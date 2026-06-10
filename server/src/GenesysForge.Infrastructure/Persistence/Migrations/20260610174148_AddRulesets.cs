using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GenesysForge.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddRulesets : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Rulesets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    Version = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rulesets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RuleEntities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RulesetId = table.Column<Guid>(type: "uuid", nullable: false),
                    EntityType = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    Key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Name = table.Column<string>(type: "character varying(160)", maxLength: 160, nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RuleEntities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RuleEntities_Rulesets_RulesetId",
                        column: x => x.RulesetId,
                        principalTable: "Rulesets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SourceBooks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RulesetId = table.Column<Guid>(type: "uuid", nullable: false),
                    Key = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    Name = table.Column<string>(type: "character varying(160)", maxLength: 160, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SourceBooks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SourceBooks_Rulesets_RulesetId",
                        column: x => x.RulesetId,
                        principalTable: "Rulesets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RuleSourceVersions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SourceBookId = table.Column<Guid>(type: "uuid", nullable: false),
                    Version = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RuleSourceVersions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RuleSourceVersions_SourceBooks_SourceBookId",
                        column: x => x.SourceBookId,
                        principalTable: "SourceBooks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RuleDefinitions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RuleEntityId = table.Column<Guid>(type: "uuid", nullable: false),
                    SourceVersionId = table.Column<Guid>(type: "uuid", nullable: false),
                    Key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ContentJson = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RuleDefinitions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RuleDefinitions_RuleEntities_RuleEntityId",
                        column: x => x.RuleEntityId,
                        principalTable: "RuleEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RuleDefinitions_RuleSourceVersions_SourceVersionId",
                        column: x => x.SourceVersionId,
                        principalTable: "RuleSourceVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Rulesets",
                columns: new[] { "Id", "Description", "Name", "Version" },
                values: new object[] { new Guid("10000000-0000-0000-0000-000000000001"), "Открытый демо-набор для проверки rules-driven данных без защищенного контента.", "Демо-набор Genesys Forge", "1.0" });

            migrationBuilder.InsertData(
                table: "RuleEntities",
                columns: new[] { "Id", "Description", "EntityType", "Key", "Name", "RulesetId" },
                values: new object[,]
                {
                    { new Guid("10000000-0000-0000-0000-000000000004"), "Защитник, который держит строй и помогает группе пережить опасность.", "archetype", "guardian", "Страж", new Guid("10000000-0000-0000-0000-000000000001") },
                    { new Guid("10000000-0000-0000-0000-000000000005"), "Исследователь необычного, полагающийся на интуицию и внутреннюю дисциплину.", "archetype", "mystic", "Мистик", new Guid("10000000-0000-0000-0000-000000000001") },
                    { new Guid("10000000-0000-0000-0000-000000000006"), "Навык спокойствия, выдержки и сопротивления давлению.", "skill", "resolve", "Стойкость", new Guid("10000000-0000-0000-0000-000000000001") },
                    { new Guid("10000000-0000-0000-0000-000000000007"), "Навык планирования действий и чтения ситуации.", "skill", "tactics", "Тактика", new Guid("10000000-0000-0000-0000-000000000001") }
                });

            migrationBuilder.InsertData(
                table: "SourceBooks",
                columns: new[] { "Id", "Key", "Name", "RulesetId" },
                values: new object[] { new Guid("10000000-0000-0000-0000-000000000002"), "demo-core", "Демо-правила", new Guid("10000000-0000-0000-0000-000000000001") });

            migrationBuilder.InsertData(
                table: "RuleSourceVersions",
                columns: new[] { "Id", "IsActive", "SourceBookId", "Version" },
                values: new object[] { new Guid("10000000-0000-0000-0000-000000000003"), true, new Guid("10000000-0000-0000-0000-000000000002"), "1.0" });

            migrationBuilder.InsertData(
                table: "RuleDefinitions",
                columns: new[] { "Id", "ContentJson", "Key", "RuleEntityId", "SourceVersionId" },
                values: new object[,]
                {
                    { new Guid("10000000-0000-0000-0000-000000000008"), "{\"woundThresholdBonus\":2,\"strainThresholdBonus\":0,\"startingSkillKeys\":[\"resolve\",\"tactics\"]}", "starting-profile", new Guid("10000000-0000-0000-0000-000000000004"), new Guid("10000000-0000-0000-0000-000000000003") },
                    { new Guid("10000000-0000-0000-0000-000000000009"), "{\"woundThresholdBonus\":0,\"strainThresholdBonus\":2,\"startingSkillKeys\":[\"resolve\"]}", "starting-profile", new Guid("10000000-0000-0000-0000-000000000005"), new Guid("10000000-0000-0000-0000-000000000003") },
                    { new Guid("10000000-0000-0000-0000-00000000000a"), "{\"characteristic\":\"willpower\",\"isCareerDefault\":false}", "skill-profile", new Guid("10000000-0000-0000-0000-000000000006"), new Guid("10000000-0000-0000-0000-000000000003") },
                    { new Guid("10000000-0000-0000-0000-00000000000b"), "{\"characteristic\":\"intellect\",\"isCareerDefault\":false}", "skill-profile", new Guid("10000000-0000-0000-0000-000000000007"), new Guid("10000000-0000-0000-0000-000000000003") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_RuleDefinitions_RuleEntityId_SourceVersionId_Key",
                table: "RuleDefinitions",
                columns: new[] { "RuleEntityId", "SourceVersionId", "Key" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RuleDefinitions_SourceVersionId",
                table: "RuleDefinitions",
                column: "SourceVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_RuleEntities_RulesetId_EntityType_Key",
                table: "RuleEntities",
                columns: new[] { "RulesetId", "EntityType", "Key" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RuleSourceVersions_SourceBookId_Version",
                table: "RuleSourceVersions",
                columns: new[] { "SourceBookId", "Version" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SourceBooks_RulesetId_Key",
                table: "SourceBooks",
                columns: new[] { "RulesetId", "Key" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RuleDefinitions");

            migrationBuilder.DropTable(
                name: "RuleEntities");

            migrationBuilder.DropTable(
                name: "RuleSourceVersions");

            migrationBuilder.DropTable(
                name: "SourceBooks");

            migrationBuilder.DropTable(
                name: "Rulesets");
        }
    }
}
