using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.NET.Migrations
{
    public partial class projects_change : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Team_Id",
                table: "Projects",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Users_Id",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Team_Id",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "Users_Id",
                table: "Projects");
        }
    }
}
