using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Taqweem.Data.Migrations
{
    public partial class PublicHolidays : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "SpecialDhuhrAdhaan",
                table: "SalaahTime",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "SpecialDhuhrSalaah",
                table: "SalaahTime",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsPublicHolidaySpecialTimesEnabled",
                table: "Masjid",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSpecialDayEnabled",
                table: "Masjid",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "SpecialDayNumber",
                table: "Masjid",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SpecialDhuhrAdhaan",
                table: "SalaahTime");

            migrationBuilder.DropColumn(
                name: "SpecialDhuhrSalaah",
                table: "SalaahTime");

            migrationBuilder.DropColumn(
                name: "IsPublicHolidaySpecialTimesEnabled",
                table: "Masjid");

            migrationBuilder.DropColumn(
                name: "IsSpecialDayEnabled",
                table: "Masjid");

            migrationBuilder.DropColumn(
                name: "SpecialDayNumber",
                table: "Masjid");
        }
    }
}
