using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Taqweem.Data.Migrations
{
    public partial class SalaahTimes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SalaahTime",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 38, nullable: false),
                    AsrAdhaan = table.Column<DateTime>(nullable: false),
                    AsrSalaah = table.Column<DateTime>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    DayNumber = table.Column<int>(nullable: false),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    DhuhrAdhaan = table.Column<DateTime>(nullable: false),
                    DhuhrSalaah = table.Column<DateTime>(nullable: false),
                    FajrAdhaan = table.Column<DateTime>(nullable: false),
                    FajrSalaah = table.Column<DateTime>(nullable: false),
                    IshaAdhaan = table.Column<DateTime>(nullable: false),
                    IshaSalaah = table.Column<DateTime>(nullable: false),
                    MasjidId = table.Column<string>(nullable: true),
                    UID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UpdatedAt = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalaahTime", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SalaahTime_Masjid_MasjidId",
                        column: x => x.MasjidId,
                        principalTable: "Masjid",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SalaahTime_MasjidId",
                table: "SalaahTime",
                column: "MasjidId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SalaahTime");
        }
    }
}
