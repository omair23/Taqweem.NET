using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Taqweem.Data.Migrations
{
    public partial class Notices : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Notice",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 38, nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    CreatedById = table.Column<string>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    IsHidden = table.Column<bool>(nullable: false),
                    MasjidId = table.Column<string>(nullable: true),
                    NoticeContent = table.Column<string>(nullable: true),
                    UID = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UpdatedAt = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notice", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notice_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Notice_Masjid_MasjidId",
                        column: x => x.MasjidId,
                        principalTable: "Masjid",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Notice_CreatedById",
                table: "Notice",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Notice_MasjidId",
                table: "Notice",
                column: "MasjidId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Notice");

            migrationBuilder.DropColumn(
                name: "FullName",
                table: "AspNetUsers");
        }
    }
}
