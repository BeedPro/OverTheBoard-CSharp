using Microsoft.EntityFrameworkCore.Migrations;

namespace OverTheBoard.Data.Migrations.ApplicationDb
{
    public partial class Checkany3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TimeRemain",
                schema: "application",
                table: "Players",
                newName: "TimeRemaining");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TimeRemaining",
                schema: "application",
                table: "Players",
                newName: "TimeRemain");
        }
    }
}
