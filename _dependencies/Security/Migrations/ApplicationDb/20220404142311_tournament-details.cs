using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OverTheBoard.Data.Migrations.ApplicationDb
{
    public partial class tournamentdetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedDate",
                schema: "application",
                table: "TournamentUsers");

            migrationBuilder.DropColumn(
                name: "isActive",
                schema: "application",
                table: "TournamentUsers");

            migrationBuilder.AlterColumn<int>(
                name: "TournamentId",
                schema: "application",
                table: "TournamentUsers",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "TEXT");

            migrationBuilder.CreateTable(
                name: "Tournaments",
                schema: "application",
                columns: table => new
                {
                    TournamentId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TournamentIdentifier = table.Column<Guid>(type: "TEXT", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    StartDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EndDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tournaments", x => x.TournamentId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TournamentUsers_TournamentId",
                schema: "application",
                table: "TournamentUsers",
                column: "TournamentId");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TournamentUsers_Tournaments_TournamentId",
                schema: "application",
                table: "TournamentUsers");

            migrationBuilder.DropTable(
                name: "Tournaments",
                schema: "application");

            migrationBuilder.DropIndex(
                name: "IX_TournamentUsers_TournamentId",
                schema: "application",
                table: "TournamentUsers");

            migrationBuilder.AlterColumn<Guid>(
                name: "TournamentId",
                schema: "application",
                table: "TournamentUsers",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                schema: "application",
                table: "TournamentUsers",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "isActive",
                schema: "application",
                table: "TournamentUsers",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }
    }
}
