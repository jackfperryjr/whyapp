using Microsoft.EntityFrameworkCore.Migrations;

namespace WhyApp.Data.Migrations
{
    public partial class ProfileAndWallpaper : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Profile",
                table: "AspNetUsers",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Wallpaper",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Profile",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Wallpaper",
                table: "AspNetUsers");
        }
    }
}
