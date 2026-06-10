using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GenesysForge.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddCharacters : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Characters",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OwnerUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RulesetId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    Status = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Characters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Characters_Rulesets_RulesetId",
                        column: x => x.RulesetId,
                        principalTable: "Rulesets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Characters_Users_OwnerUserId",
                        column: x => x.OwnerUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CharacterSkills",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CharacterId = table.Column<Guid>(type: "uuid", nullable: false),
                    RuleEntityId = table.Column<Guid>(type: "uuid", nullable: false),
                    Rank = table.Column<int>(type: "integer", nullable: false),
                    XpSpent = table.Column<int>(type: "integer", nullable: false),
                    IsCareerSkill = table.Column<bool>(type: "boolean", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterSkills", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CharacterSkills_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CharacterSkills_RuleEntities_RuleEntityId",
                        column: x => x.RuleEntityId,
                        principalTable: "RuleEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CharacterSnapshots",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CharacterId = table.Column<Guid>(type: "uuid", nullable: false),
                    RulesetId = table.Column<Guid>(type: "uuid", nullable: false),
                    ContentJson = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterSnapshots", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CharacterSnapshots_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CharacterSnapshots_Rulesets_RulesetId",
                        column: x => x.RulesetId,
                        principalTable: "Rulesets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CharacterTalents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CharacterId = table.Column<Guid>(type: "uuid", nullable: false),
                    RuleEntityId = table.Column<Guid>(type: "uuid", nullable: false),
                    XpCost = table.Column<int>(type: "integer", nullable: false),
                    PurchasedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterTalents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CharacterTalents_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CharacterTalents_RuleEntities_RuleEntityId",
                        column: x => x.RuleEntityId,
                        principalTable: "RuleEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "XpLedgerEntries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CharacterId = table.Column<Guid>(type: "uuid", nullable: false),
                    Amount = table.Column<int>(type: "integer", nullable: false),
                    Reason = table.Column<string>(type: "character varying(240)", maxLength: 240, nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_XpLedgerEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_XpLedgerEntries_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Characters_OwnerUserId",
                table: "Characters",
                column: "OwnerUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Characters_RulesetId",
                table: "Characters",
                column: "RulesetId");

            migrationBuilder.CreateIndex(
                name: "IX_CharacterSkills_CharacterId_RuleEntityId",
                table: "CharacterSkills",
                columns: new[] { "CharacterId", "RuleEntityId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CharacterSkills_RuleEntityId",
                table: "CharacterSkills",
                column: "RuleEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_CharacterSnapshots_CharacterId",
                table: "CharacterSnapshots",
                column: "CharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_CharacterSnapshots_RulesetId",
                table: "CharacterSnapshots",
                column: "RulesetId");

            migrationBuilder.CreateIndex(
                name: "IX_CharacterTalents_CharacterId_RuleEntityId",
                table: "CharacterTalents",
                columns: new[] { "CharacterId", "RuleEntityId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CharacterTalents_RuleEntityId",
                table: "CharacterTalents",
                column: "RuleEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_XpLedgerEntries_CharacterId",
                table: "XpLedgerEntries",
                column: "CharacterId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CharacterSkills");

            migrationBuilder.DropTable(
                name: "CharacterSnapshots");

            migrationBuilder.DropTable(
                name: "CharacterTalents");

            migrationBuilder.DropTable(
                name: "XpLedgerEntries");

            migrationBuilder.DropTable(
                name: "Characters");
        }
    }
}
