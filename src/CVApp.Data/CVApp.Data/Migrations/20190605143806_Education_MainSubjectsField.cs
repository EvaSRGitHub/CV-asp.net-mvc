using Microsoft.EntityFrameworkCore.Migrations;

namespace CVApp.Data.Migrations
{
    public partial class Education_MainSubjectsField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MainSubjects",
                table: "Educations",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MainSubjects",
                table: "Educations");
        }
    }
}
