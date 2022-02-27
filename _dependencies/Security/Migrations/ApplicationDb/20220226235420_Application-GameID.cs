using Microsoft.EntityFrameworkCore.Migrations;

namespace OverTheBoard.Data.Migrations.ApplicationDb
{
    public partial class ApplicationGameID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Players_Games_PlayerId",
                schema: "application",
                table: "Players");

            migrationBuilder.AlterColumn<int>(
                name: "PlayerId",
                schema: "application",
                table: "Players",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddColumn<int>(
                name: "GameId",
                schema: "application",
                table: "Players",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Players_GameId",
                schema: "application",
                table: "Players",
                column: "GameId");

            migrationBuilder.AddForeignKey(
                name: "FK_Players_Games_GameId",
                schema: "application",
                table: "Players",
                column: "GameId",
                principalSchema: "application",
                principalTable: "Games",
                principalColumn: "GameId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Players_Games_GameId",
                schema: "application",
                table: "Players");

            migrationBuilder.DropIndex(
                name: "IX_Players_GameId",
                schema: "application",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "GameId",
                schema: "application",
                table: "Players");

            migrationBuilder.AlterColumn<int>(
                name: "PlayerId",
                schema: "application",
                table: "Players",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddForeignKey(
                name: "FK_Players_Games_PlayerId",
                schema: "application",
                table: "Players",
                column: "PlayerId",
                principalSchema: "application",
                principalTable: "Games",
                principalColumn: "GameId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
