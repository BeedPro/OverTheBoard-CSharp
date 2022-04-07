using Microsoft.EntityFrameworkCore.Migrations;

namespace OverTheBoard.Data.Migrations.ApplicationDb
{
    public partial class tournamentPlayer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TournamentUserId",
                schema: "application",
                table: "TournamentUsers",
                newName: "TournamentPlayerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TournamentPlayerId",
                schema: "application",
                table: "TournamentUsers",
                newName: "TournamentUserId");
        }
    }
}
