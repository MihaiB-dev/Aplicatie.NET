using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.NET.Data.Migrations
{
    public partial class ajutorr : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Id",
                table: "Team_members");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Team_members",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
