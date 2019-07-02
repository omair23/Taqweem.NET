using Microsoft.EntityFrameworkCore.Migrations;

namespace Taqweem.Data.Migrations
{
    public partial class NextTimeChangeCheck : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAsrTimeChange",
                schema: "Taqweem",
                table: "SalaahTime",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDhuhrTimeChange",
                schema: "Taqweem",
                table: "SalaahTime",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsFajrTimeChange",
                schema: "Taqweem",
                table: "SalaahTime",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsIshaTimeChange",
                schema: "Taqweem",
                table: "SalaahTime",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsJumuahTimeChange",
                schema: "Taqweem",
                table: "SalaahTime",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSpecialDhuhrTimeChange",
                schema: "Taqweem",
                table: "SalaahTime",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAsrTimeChange",
                schema: "Taqweem",
                table: "SalaahTime");

            migrationBuilder.DropColumn(
                name: "IsDhuhrTimeChange",
                schema: "Taqweem",
                table: "SalaahTime");

            migrationBuilder.DropColumn(
                name: "IsFajrTimeChange",
                schema: "Taqweem",
                table: "SalaahTime");

            migrationBuilder.DropColumn(
                name: "IsIshaTimeChange",
                schema: "Taqweem",
                table: "SalaahTime");

            migrationBuilder.DropColumn(
                name: "IsJumuahTimeChange",
                schema: "Taqweem",
                table: "SalaahTime");

            migrationBuilder.DropColumn(
                name: "IsSpecialDhuhrTimeChange",
                schema: "Taqweem",
                table: "SalaahTime");
        }
    }
}
