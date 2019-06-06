using System;
using Microsoft.EntityFrameworkCore.Metadata;
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

            migrationBuilder.CreateTable(
                name: "PublicHoliday",
                schema: "World",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 38, nullable: false),
                    UID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<string>(maxLength: 38, nullable: true),
                    DayOfHoliday = table.Column<DateTime>(nullable: false),
                    NameOfHoliday = table.Column<string>(nullable: true),
                    Country = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PublicHoliday", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PublicHoliday",
                schema: "World");

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
