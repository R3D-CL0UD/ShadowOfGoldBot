using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShadowOfGoldBot.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Endings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    ImageUrl = table.Column<string>(type: "TEXT", nullable: true),
                    Condition = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Endings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Scenes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    ImageUrl = table.Column<string>(type: "TEXT", nullable: true),
                    VoiceUrl = table.Column<string>(type: "TEXT", nullable: true),
                    IsEnding = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Scenes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Choices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SceneId = table.Column<int>(type: "INTEGER", nullable: false),
                    Text = table.Column<string>(type: "TEXT", nullable: false),
                    NextSceneId = table.Column<int>(type: "INTEGER", nullable: false),
                    ReputationTownChange = table.Column<int>(type: "INTEGER", nullable: false),
                    ReputationSheriffChange = table.Column<int>(type: "INTEGER", nullable: false),
                    ReputationGangChange = table.Column<int>(type: "INTEGER", nullable: false),
                    TrustMayChange = table.Column<int>(type: "INTEGER", nullable: false),
                    KarmaGreedChange = table.Column<int>(type: "INTEGER", nullable: false),
                    FlagToSet = table.Column<string>(type: "TEXT", nullable: true),
                    RequiredFlag = table.Column<string>(type: "TEXT", nullable: true),
                    RequiredItem = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Choices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Choices_Scenes_SceneId",
                        column: x => x.SceneId,
                        principalTable: "Scenes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TelegramUserId = table.Column<long>(type: "INTEGER", nullable: false),
                    FirstName = table.Column<string>(type: "TEXT", nullable: true),
                    Username = table.Column<string>(type: "TEXT", nullable: true),
                    CurrentSceneId = table.Column<int>(type: "INTEGER", nullable: false),
                    IsGameFinished = table.Column<bool>(type: "INTEGER", nullable: false),
                    EndingId = table.Column<int>(type: "INTEGER", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastPlayedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Players_Endings_EndingId",
                        column: x => x.EndingId,
                        principalTable: "Endings",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Players_Scenes_CurrentSceneId",
                        column: x => x.CurrentSceneId,
                        principalTable: "Scenes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlayerFlags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PlayerId = table.Column<int>(type: "INTEGER", nullable: false),
                    FlagName = table.Column<string>(type: "TEXT", nullable: false),
                    FlagValue = table.Column<string>(type: "TEXT", nullable: false),
                    SetAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerFlags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayerFlags_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlayerStates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PlayerId = table.Column<int>(type: "INTEGER", nullable: false),
                    ReputationTown = table.Column<int>(type: "INTEGER", nullable: false),
                    ReputationSheriff = table.Column<int>(type: "INTEGER", nullable: false),
                    ReputationGang = table.Column<int>(type: "INTEGER", nullable: false),
                    TrustMay = table.Column<int>(type: "INTEGER", nullable: false),
                    KarmaGreed = table.Column<int>(type: "INTEGER", nullable: false),
                    HasGold = table.Column<bool>(type: "INTEGER", nullable: false),
                    HasRifle = table.Column<bool>(type: "INTEGER", nullable: false),
                    HasAmmo = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerStates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayerStates_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Choices_SceneId",
                table: "Choices",
                column: "SceneId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerFlags_PlayerId",
                table: "PlayerFlags",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_Players_CurrentSceneId",
                table: "Players",
                column: "CurrentSceneId");

            migrationBuilder.CreateIndex(
                name: "IX_Players_EndingId",
                table: "Players",
                column: "EndingId");

            migrationBuilder.CreateIndex(
                name: "IX_Players_TelegramUserId",
                table: "Players",
                column: "TelegramUserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlayerStates_PlayerId",
                table: "PlayerStates",
                column: "PlayerId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Choices");

            migrationBuilder.DropTable(
                name: "PlayerFlags");

            migrationBuilder.DropTable(
                name: "PlayerStates");

            migrationBuilder.DropTable(
                name: "Players");

            migrationBuilder.DropTable(
                name: "Endings");

            migrationBuilder.DropTable(
                name: "Scenes");
        }
    }
}
