using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.NET.Data.Migrations
{
    public partial class Nume : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Comments_CommentId",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_CommentId",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "date",
                table: "Tasks");

            migrationBuilder.RenameColumn(
                name: "CommentId",
                table: "Tasks",
                newName: "Project_id");

            migrationBuilder.AlterColumn<string>(
                name: "Description_task",
                table: "Tasks",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<DateTime>(
                name: "Data_end",
                table: "Tasks",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Data_start",
                table: "Tasks",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "User_id",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "description",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "password",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "title",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Id_task",
                table: "Comments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TaskId",
                table: "Comments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comments_TaskId",
                table: "Comments",
                column: "TaskId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Tasks_TaskId",
                table: "Comments",
                column: "TaskId",
                principalTable: "Tasks",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Tasks_TaskId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_TaskId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "Data_end",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "Data_start",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "User_id",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "description",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "password",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "title",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "Id_task",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "TaskId",
                table: "Comments");

            migrationBuilder.RenameColumn(
                name: "Project_id",
                table: "Tasks",
                newName: "CommentId");

            migrationBuilder.AlterColumn<string>(
                name: "Description_task",
                table: "Tasks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "date",
                table: "Tasks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_CommentId",
                table: "Tasks",
                column: "CommentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Comments_CommentId",
                table: "Tasks",
                column: "CommentId",
                principalTable: "Comments",
                principalColumn: "Id");
        }
    }
}
