using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OverTheBoard.Data.Migrations.ApplicationDb
{
    public partial class AddedTournamentUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "GroupIdentifier",
                schema: "application",
                table: "Games",
                newName: "TournamentId");

            migrationBuilder.CreateTable(
                name: "TournamentUsers",
                schema: "application",
                columns: table => new
                {
                    TournamentUserId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TournamentId = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    isActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TournamentUsers", x => x.TournamentUserId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TournamentUsers",
                schema: "application");

            migrationBuilder.RenameColumn(
                name: "TournamentId",
                schema: "application",
                table: "Games",
                newName: "GroupIdentifier");
        }
    }
}
