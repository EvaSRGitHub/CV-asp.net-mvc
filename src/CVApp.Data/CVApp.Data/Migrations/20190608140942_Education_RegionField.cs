using Microsoft.EntityFrameworkCore.Migrations;

namespace CVApp.Data.Migrations
{
    public partial class Education_RegionField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Region",
                table: "Educations",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Region",
                table: "Educations");
        }
    }
}
