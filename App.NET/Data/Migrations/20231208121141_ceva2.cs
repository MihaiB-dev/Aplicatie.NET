using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.NET.Data.Migrations
{
    public partial class ceva2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "User_task",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Team_member",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Id",
                table: "User_task");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Team_member");
        }
    }
}
