using Microsoft.EntityFrameworkCore.Migrations;

namespace OverTheBoard.Data.Migrations
{
    public partial class AddedRanks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Rank",
                schema: "identity",
                table: "AspNetUsers",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rank",
                schema: "identity",
                table: "AspNetUsers");
        }
    }
}
