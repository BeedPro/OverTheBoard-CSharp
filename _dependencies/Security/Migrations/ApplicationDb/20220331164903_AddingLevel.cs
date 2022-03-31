using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OverTheBoard.Data.Migrations.ApplicationDb
{
    public partial class AddingLevel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RankedGameQueue",
                schema: "application");

            migrationBuilder.AddColumn<int>(
                name: "Level",
                schema: "application",
                table: "Games",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Level",
                schema: "application",
                table: "CompletionQueue",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "TournamentQueue",
                schema: "application",
                columns: table => new
                {
                    RankedGameQueueId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Identifier = table.Column<Guid>(type: "TEXT", nullable: false),
                    Level = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TournamentQueue", x => x.RankedGameQueueId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TournamentQueue",
                schema: "application");

            migrationBuilder.DropColumn(
                name: "Level",
                schema: "application",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "Level",
                schema: "application",
                table: "CompletionQueue");

            migrationBuilder.CreateTable(
                name: "RankedGameQueue",
                schema: "application",
                columns: table => new
                {
                    RankedGameQueueId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Identifier = table.Column<Guid>(type: "TEXT", nullable: false),
                    Level = table.Column<int>(type: "INTEGER", nullable: false),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RankedGameQueue", x => x.RankedGameQueueId);
                });
        }
    }
}
