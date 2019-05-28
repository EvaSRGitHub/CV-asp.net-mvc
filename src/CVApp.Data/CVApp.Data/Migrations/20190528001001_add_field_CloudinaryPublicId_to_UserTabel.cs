using Microsoft.EntityFrameworkCore.Migrations;

namespace CVApp.Data.Migrations
{
    public partial class add_field_CloudinaryPublicId_to_UserTabel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CloudinaryPublicId",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CloudinaryPublicId",
                table: "AspNetUsers");
        }
    }
}
