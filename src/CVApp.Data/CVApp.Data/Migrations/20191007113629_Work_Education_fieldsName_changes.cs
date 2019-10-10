using Microsoft.EntityFrameworkCore.Migrations;

namespace CVApp.Data.Migrations
{
    public partial class Work_Education_fieldsName_changes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ToYear",
                table: "Works",
                newName: "EndDate");

            migrationBuilder.RenameColumn(
                name: "FromYear",
                table: "Works",
                newName: "StartDate");

            migrationBuilder.RenameColumn(
                name: "ToYear",
                table: "Educations",
                newName: "StartDate");

            migrationBuilder.RenameColumn(
                name: "FromYear",
                table: "Educations",
                newName: "EndDate");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "Works",
                newName: "FromYear");

            migrationBuilder.RenameColumn(
                name: "EndDate",
                table: "Works",
                newName: "ToYear");

            migrationBuilder.RenameColumn(
                name: "StartDate",
                table: "Educations",
                newName: "ToYear");

            migrationBuilder.RenameColumn(
                name: "EndDate",
                table: "Educations",
                newName: "FromYear");
        }
    }
}
