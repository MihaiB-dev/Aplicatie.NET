using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.NET.Data.Migrations
{
    public partial class Incercare : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Badges_Scores_ScoreId",
                table: "Badges");

            migrationBuilder.DropColumn(
                name: "score",
                table: "Scores");

            migrationBuilder.DropColumn(
                name: "team_member_id",
                table: "Scores");

            migrationBuilder.DropColumn(
                name: "Id_user",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "score_id",
                table: "Badges");

            migrationBuilder.DropColumn(
                name: "title_badge",
                table: "Badges");

            migrationBuilder.RenameColumn(
                name: "bonus",
                table: "Scores",
                newName: "Bonus");

            migrationBuilder.RenameColumn(
                name: "date",
                table: "Scores",
                newName: "DateScore");

            migrationBuilder.RenameColumn(
                name: "date",
                table: "Comments",
                newName: "Date");

            migrationBuilder.RenameColumn(
                name: "total_points",
                table: "AspNetUsers",
                newName: "Total_points");

            migrationBuilder.AddColumn<int>(
                name: "Points",
                table: "Scores",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Team_memberId",
                table: "Scores",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "User_id",
                table: "Comments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ScoreId",
                table: "Badges",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TitleBadge",
                table: "Badges",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Total_points",
                table: "AspNetUsers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Badges_Scores_ScoreId",
                table: "Badges",
                column: "ScoreId",
                principalTable: "Scores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Badges_Scores_ScoreId",
                table: "Badges");

            migrationBuilder.DropColumn(
                name: "Points",
                table: "Scores");

            migrationBuilder.DropColumn(
                name: "Team_memberId",
                table: "Scores");

            migrationBuilder.DropColumn(
                name: "User_id",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "TitleBadge",
                table: "Badges");

            migrationBuilder.RenameColumn(
                name: "Bonus",
                table: "Scores",
                newName: "bonus");

            migrationBuilder.RenameColumn(
                name: "DateScore",
                table: "Scores",
                newName: "date");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "Comments",
                newName: "date");

            migrationBuilder.RenameColumn(
                name: "Total_points",
                table: "AspNetUsers",
                newName: "total_points");

            migrationBuilder.AddColumn<int>(
                name: "score",
                table: "Scores",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "team_member_id",
                table: "Scores",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Id_user",
                table: "Comments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "ScoreId",
                table: "Badges",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "score_id",
                table: "Badges",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "title_badge",
                table: "Badges",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "total_points",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Badges_Scores_ScoreId",
                table: "Badges",
                column: "ScoreId",
                principalTable: "Scores",
                principalColumn: "Id");
        }
    }
}
