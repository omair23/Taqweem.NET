using Microsoft.EntityFrameworkCore.Migrations;

namespace Taqweem.Data.Migrations
{
    public partial class TZTimezone : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TZTimeZone",
                schema: "Taqweem",
                table: "Masjid",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TZTimeZone",
                schema: "Taqweem",
                table: "Masjid");
        }
    }
}
