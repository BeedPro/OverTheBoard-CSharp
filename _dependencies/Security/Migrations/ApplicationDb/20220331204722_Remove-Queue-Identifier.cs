using Microsoft.EntityFrameworkCore.Migrations;

namespace OverTheBoard.Data.Migrations.ApplicationDb
{
    public partial class RemoveQueueIdentifier : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Identifier",
                schema: "application",
                table: "TournamentQueue",
                newName: "CreatedDate");

            migrationBuilder.RenameColumn(
                name: "RankedGameQueueId",
                schema: "application",
                table: "TournamentQueue",
                newName: "TournamentQueueId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                schema: "application",
                table: "TournamentQueue",
                newName: "Identifier");

            migrationBuilder.RenameColumn(
                name: "TournamentQueueId",
                schema: "application",
                table: "TournamentQueue",
                newName: "RankedGameQueueId");
        }
    }
}
