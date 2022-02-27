using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OverTheBoard.Data.Migrations.ApplicationDb
{
    public partial class Applicationinit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "application");

            migrationBuilder.CreateTable(
                name: "Games",
                schema: "application",
                columns: table => new
                {
                    GameId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Identifier = table.Column<Guid>(type: "TEXT", nullable: false),
                    Fen = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.GameId);
                });

            migrationBuilder.CreateTable(
                name: "Players",
                schema: "application",
                columns: table => new
                {
                    PlayerId = table.Column<int>(type: "INTEGER", nullable: false),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ConnectionId = table.Column<string>(type: "TEXT", nullable: true),
                    Colour = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.PlayerId);
                    table.ForeignKey(
                        name: "FK_Players_Games_PlayerId",
                        column: x => x.PlayerId,
                        principalSchema: "application",
                        principalTable: "Games",
                        principalColumn: "GameId",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Players",
                schema: "application");

            migrationBuilder.DropTable(
                name: "Games",
                schema: "application");
        }
    }
}
