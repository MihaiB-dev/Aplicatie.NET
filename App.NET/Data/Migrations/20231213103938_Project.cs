using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.NET.Data.Migrations
{
    public partial class Project : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "title",
                table: "Projects");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "Projects",
                newName: "Description");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Projects",
                newName: "description");

            migrationBuilder.AddColumn<string>(
                name: "title",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
