using Microsoft.EntityFrameworkCore.Migrations;

namespace Taqweem.Data.Migrations
{
    public partial class NewSchema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Taqweem");

            migrationBuilder.RenameTable(
                name: "TimeZone",
                newName: "TimeZone",
                newSchema: "Taqweem");

            migrationBuilder.RenameTable(
                name: "SalaahTime",
                newName: "SalaahTime",
                newSchema: "Taqweem");

            migrationBuilder.RenameTable(
                name: "Notice",
                newName: "Notice",
                newSchema: "Taqweem");

            migrationBuilder.RenameTable(
                name: "Masjid",
                newName: "Masjid",
                newSchema: "Taqweem");

            migrationBuilder.RenameTable(
                name: "Configuration",
                newName: "Configuration",
                newSchema: "Taqweem");

            migrationBuilder.RenameTable(
                name: "AspNetUserTokens",
                newName: "AspNetUserTokens",
                newSchema: "Taqweem");

            migrationBuilder.RenameTable(
                name: "AspNetUsers",
                newName: "AspNetUsers",
                newSchema: "Taqweem");

            migrationBuilder.RenameTable(
                name: "AspNetUserRoles",
                newName: "AspNetUserRoles",
                newSchema: "Taqweem");

            migrationBuilder.RenameTable(
                name: "AspNetUserLogins",
                newName: "AspNetUserLogins",
                newSchema: "Taqweem");

            migrationBuilder.RenameTable(
                name: "AspNetUserClaims",
                newName: "AspNetUserClaims",
                newSchema: "Taqweem");

            migrationBuilder.RenameTable(
                name: "AspNetRoles",
                newName: "AspNetRoles",
                newSchema: "Taqweem");

            migrationBuilder.RenameTable(
                name: "AspNetRoleClaims",
                newName: "AspNetRoleClaims",
                newSchema: "Taqweem");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "TimeZone",
                schema: "Taqweem",
                newName: "TimeZone");

            migrationBuilder.RenameTable(
                name: "SalaahTime",
                schema: "Taqweem",
                newName: "SalaahTime");

            migrationBuilder.RenameTable(
                name: "Notice",
                schema: "Taqweem",
                newName: "Notice");

            migrationBuilder.RenameTable(
                name: "Masjid",
                schema: "Taqweem",
                newName: "Masjid");

            migrationBuilder.RenameTable(
                name: "Configuration",
                schema: "Taqweem",
                newName: "Configuration");

            migrationBuilder.RenameTable(
                name: "AspNetUserTokens",
                schema: "Taqweem",
                newName: "AspNetUserTokens");

            migrationBuilder.RenameTable(
                name: "AspNetUsers",
                schema: "Taqweem",
                newName: "AspNetUsers");

            migrationBuilder.RenameTable(
                name: "AspNetUserRoles",
                schema: "Taqweem",
                newName: "AspNetUserRoles");

            migrationBuilder.RenameTable(
                name: "AspNetUserLogins",
                schema: "Taqweem",
                newName: "AspNetUserLogins");

            migrationBuilder.RenameTable(
                name: "AspNetUserClaims",
                schema: "Taqweem",
                newName: "AspNetUserClaims");

            migrationBuilder.RenameTable(
                name: "AspNetRoles",
                schema: "Taqweem",
                newName: "AspNetRoles");

            migrationBuilder.RenameTable(
                name: "AspNetRoleClaims",
                schema: "Taqweem",
                newName: "AspNetRoleClaims");
        }
    }
}
