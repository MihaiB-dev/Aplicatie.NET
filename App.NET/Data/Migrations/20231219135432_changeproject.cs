using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.NET.Data.Migrations
{
    public partial class changeproject : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Team_id",
                table: "Projects");

            migrationBuilder.RenameColumn(
                name: "User_id",
                table: "Projects",
                newName: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Projects",
                newName: "User_id");

            migrationBuilder.AddColumn<int>(
                name: "Team_id",
                table: "Projects",
                type: "int",
                nullable: true);
        }
    }
}
