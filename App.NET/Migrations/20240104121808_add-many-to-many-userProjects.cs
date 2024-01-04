using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.NET.Migrations
{
    public partial class addmanytomanyuserProjects : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserProjects",
                columns: table => new
                {
                    User_id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Project_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProjects", x => new { x.User_id, x.Project_id });
                    table.ForeignKey(
                        name: "FK_UserProjects_AspNetUsers_User_id",
                        column: x => x.User_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserProjects_Projects_Project_id",
                        column: x => x.Project_id,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserProjects_Project_id",
                table: "UserProjects",
                column: "Project_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserProjects");
        }
    }
}
