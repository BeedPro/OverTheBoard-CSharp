using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OverTheBoard.Data.Migrations.ApplicationDb
{
    public partial class Addingtimer001 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastMoveAt",
                schema: "application",
                table: "Players");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastMoveAt",
                schema: "application",
                table: "Games",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastMoveAt",
                schema: "application",
                table: "Games");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastMoveAt",
                schema: "application",
                table: "Players",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
