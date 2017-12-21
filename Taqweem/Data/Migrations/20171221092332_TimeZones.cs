using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Taqweem.Data.Migrations
{
    public partial class TimeZones : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TimeZone",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    DaylightName = table.Column<string>(nullable: true),
                    DefaultUTCDifference = table.Column<double>(nullable: false),
                    DisplayName = table.Column<string>(nullable: true),
                    StandardName = table.Column<string>(nullable: true),
                    SupportsDaylightSavingTime = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeZone", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TimeZone");
        }
    }
}
