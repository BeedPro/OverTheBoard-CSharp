using Microsoft.EntityFrameworkCore.Migrations;

namespace OverTheBoard.Data.Migrations.ApplicationDb
{
    public partial class tournamentPlayer1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TournamentUsers_Tournaments_TournamentId",
                schema: "application",
                table: "TournamentUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TournamentUsers",
                schema: "application",
                table: "TournamentUsers");

            migrationBuilder.RenameTable(
                name: "TournamentUsers",
                schema: "application",
                newName: "TournamentPlayers",
                newSchema: "application");

            migrationBuilder.RenameIndex(
                name: "IX_TournamentUsers_TournamentId",
                schema: "application",
                table: "TournamentPlayers",
                newName: "IX_TournamentPlayers_TournamentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TournamentPlayers",
                schema: "application",
                table: "TournamentPlayers",
                column: "TournamentPlayerId");

            migrationBuilder.AddForeignKey(
                name: "FK_TournamentPlayers_Tournaments_TournamentId",
                schema: "application",
                table: "TournamentPlayers",
                column: "TournamentId",
                principalSchema: "application",
                principalTable: "Tournaments",
                principalColumn: "TournamentId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TournamentPlayers_Tournaments_TournamentId",
                schema: "application",
                table: "TournamentPlayers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TournamentPlayers",
                schema: "application",
                table: "TournamentPlayers");

            migrationBuilder.RenameTable(
                name: "TournamentPlayers",
                schema: "application",
                newName: "TournamentUsers",
                newSchema: "application");

            migrationBuilder.RenameIndex(
                name: "IX_TournamentPlayers_TournamentId",
                schema: "application",
                table: "TournamentUsers",
                newName: "IX_TournamentUsers_TournamentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TournamentUsers",
                schema: "application",
                table: "TournamentUsers",
                column: "TournamentPlayerId");

            migrationBuilder.AddForeignKey(
                name: "FK_TournamentUsers_Tournaments_TournamentId",
                schema: "application",
                table: "TournamentUsers",
                column: "TournamentId",
                principalSchema: "application",
                principalTable: "Tournaments",
                principalColumn: "TournamentId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
