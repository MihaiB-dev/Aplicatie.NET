using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.NET.Data.Migrations
{
    public partial class Ceva : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_teams_TeamId",
                table: "Projects");

            migrationBuilder.DropForeignKey(
                name: "FK_Scores_Team_members_Team_memberUser_id_Team_memberTeam_id",
                table: "Scores");

            migrationBuilder.DropForeignKey(
                name: "FK_Team_members_AspNetUsers_User_id",
                table: "Team_members");

            migrationBuilder.DropForeignKey(
                name: "FK_Team_members_teams_Team_id",
                table: "Team_members");

            migrationBuilder.DropForeignKey(
                name: "FK_User_tasks_AspNetUsers_User_id",
                table: "User_tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_User_tasks_Tasks_Task_id",
                table: "User_tasks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_User_tasks",
                table: "User_tasks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_teams",
                table: "teams");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Team_members",
                table: "Team_members");

            migrationBuilder.DropColumn(
                name: "password",
                table: "Projects");

            migrationBuilder.RenameTable(
                name: "User_tasks",
                newName: "User_task");

            migrationBuilder.RenameTable(
                name: "teams",
                newName: "Team");

            migrationBuilder.RenameTable(
                name: "Team_members",
                newName: "Team_member");

            migrationBuilder.RenameIndex(
                name: "IX_User_tasks_Task_id",
                table: "User_task",
                newName: "IX_User_task_Task_id");

            migrationBuilder.RenameIndex(
                name: "IX_Team_members_Team_id",
                table: "Team_member",
                newName: "IX_Team_member_Team_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_User_task",
                table: "User_task",
                columns: new[] { "User_id", "Task_id" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Team",
                table: "Team",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Team_member",
                table: "Team_member",
                columns: new[] { "User_id", "Team_id" });

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Team_TeamId",
                table: "Projects",
                column: "TeamId",
                principalTable: "Team",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Scores_Team_member_Team_memberUser_id_Team_memberTeam_id",
                table: "Scores",
                columns: new[] { "Team_memberUser_id", "Team_memberTeam_id" },
                principalTable: "Team_member",
                principalColumns: new[] { "User_id", "Team_id" });

            migrationBuilder.AddForeignKey(
                name: "FK_Team_member_AspNetUsers_User_id",
                table: "Team_member",
                column: "User_id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Team_member_Team_Team_id",
                table: "Team_member",
                column: "Team_id",
                principalTable: "Team",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_User_task_AspNetUsers_User_id",
                table: "User_task",
                column: "User_id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_User_task_Tasks_Task_id",
                table: "User_task",
                column: "Task_id",
                principalTable: "Tasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Team_TeamId",
                table: "Projects");

            migrationBuilder.DropForeignKey(
                name: "FK_Scores_Team_member_Team_memberUser_id_Team_memberTeam_id",
                table: "Scores");

            migrationBuilder.DropForeignKey(
                name: "FK_Team_member_AspNetUsers_User_id",
                table: "Team_member");

            migrationBuilder.DropForeignKey(
                name: "FK_Team_member_Team_Team_id",
                table: "Team_member");

            migrationBuilder.DropForeignKey(
                name: "FK_User_task_AspNetUsers_User_id",
                table: "User_task");

            migrationBuilder.DropForeignKey(
                name: "FK_User_task_Tasks_Task_id",
                table: "User_task");

            migrationBuilder.DropPrimaryKey(
                name: "PK_User_task",
                table: "User_task");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Team_member",
                table: "Team_member");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Team",
                table: "Team");

            migrationBuilder.RenameTable(
                name: "User_task",
                newName: "User_tasks");

            migrationBuilder.RenameTable(
                name: "Team_member",
                newName: "Team_members");

            migrationBuilder.RenameTable(
                name: "Team",
                newName: "teams");

            migrationBuilder.RenameIndex(
                name: "IX_User_task_Task_id",
                table: "User_tasks",
                newName: "IX_User_tasks_Task_id");

            migrationBuilder.RenameIndex(
                name: "IX_Team_member_Team_id",
                table: "Team_members",
                newName: "IX_Team_members_Team_id");

            migrationBuilder.AddColumn<string>(
                name: "password",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_User_tasks",
                table: "User_tasks",
                columns: new[] { "User_id", "Task_id" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Team_members",
                table: "Team_members",
                columns: new[] { "User_id", "Team_id" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_teams",
                table: "teams",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_teams_TeamId",
                table: "Projects",
                column: "TeamId",
                principalTable: "teams",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Scores_Team_members_Team_memberUser_id_Team_memberTeam_id",
                table: "Scores",
                columns: new[] { "Team_memberUser_id", "Team_memberTeam_id" },
                principalTable: "Team_members",
                principalColumns: new[] { "User_id", "Team_id" });

            migrationBuilder.AddForeignKey(
                name: "FK_Team_members_AspNetUsers_User_id",
                table: "Team_members",
                column: "User_id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Team_members_teams_Team_id",
                table: "Team_members",
                column: "Team_id",
                principalTable: "teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_User_tasks_AspNetUsers_User_id",
                table: "User_tasks",
                column: "User_id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_User_tasks_Tasks_Task_id",
                table: "User_tasks",
                column: "Task_id",
                principalTable: "Tasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
