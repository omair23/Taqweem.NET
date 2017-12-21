using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Taqweem.Data.Migrations
{
    public partial class TimeZonesChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TimeZone",
                table: "Masjid",
                newName: "TimeZoneDiff");

            migrationBuilder.AlterColumn<string>(
                name: "TimeZoneId",
                table: "Masjid",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Masjid_TimeZoneId",
                table: "Masjid",
                column: "TimeZoneId");

            migrationBuilder.AddForeignKey(
                name: "FK_Masjid_TimeZone_TimeZoneId",
                table: "Masjid",
                column: "TimeZoneId",
                principalTable: "TimeZone",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Masjid_TimeZone_TimeZoneId",
                table: "Masjid");

            migrationBuilder.DropIndex(
                name: "IX_Masjid_TimeZoneId",
                table: "Masjid");

            migrationBuilder.RenameColumn(
                name: "TimeZoneDiff",
                table: "Masjid",
                newName: "TimeZone");

            migrationBuilder.AlterColumn<string>(
                name: "TimeZoneId",
                table: "Masjid",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
