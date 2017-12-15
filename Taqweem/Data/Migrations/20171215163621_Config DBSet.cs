using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Taqweem.Data.Migrations
{
    public partial class ConfigDBSet : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MasjidId",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Configuration",
                columns: table => new
                {
                    UID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Parameter = table.Column<string>(nullable: true),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Configuration", x => x.UID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_MasjidId",
                table: "AspNetUsers",
                column: "MasjidId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Masjid_MasjidId",
                table: "AspNetUsers",
                column: "MasjidId",
                principalTable: "Masjid",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Masjid_MasjidId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Configuration");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_MasjidId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "MasjidId",
                table: "AspNetUsers");
        }
    }
}
