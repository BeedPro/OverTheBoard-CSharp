using Microsoft.EntityFrameworkCore.Migrations;

namespace OverTheBoard.Data.Migrations
{
    public partial class AddedRatingToUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Rating",
                schema: "identity",
                table: "AspNetUsers",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rating",
                schema: "identity",
                table: "AspNetUsers");
        }
    }
}
