using Microsoft.EntityFrameworkCore.Migrations;

namespace OverTheBoard.Core.Security.Migrations
{
    public partial class AddedDisplayImagePath1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DisplayImagePath",
                table: "AspNetUsers",
                type: "TEXT",
                maxLength: 100,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DisplayImagePath",
                table: "AspNetUsers");
        }
    }
}
