using Microsoft.EntityFrameworkCore.Migrations;

namespace OverTheBoard.Core.Security.Migrations
{
    public partial class AddedDisplayNameId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DisplayNameId",
                table: "AspNetUsers",
                type: "TEXT",
                maxLength: 100,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DisplayNameId",
                table: "AspNetUsers");
        }
    }
}
