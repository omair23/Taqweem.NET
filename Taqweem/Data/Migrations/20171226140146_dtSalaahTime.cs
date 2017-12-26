using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Taqweem.Data.Migrations
{
    public partial class dtSalaahTime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "TimeDate",
                table: "SalaahTime",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "SalaahTimeViewModel",
                columns: table => new
                {
                    KeyId = table.Column<string>(nullable: false),
                    AsrAdhaan = table.Column<DateTime>(nullable: false),
                    AsrSalaah = table.Column<DateTime>(nullable: false),
                    DhuhrAdhaan = table.Column<DateTime>(nullable: false),
                    DhuhrSalaah = table.Column<DateTime>(nullable: false),
                    EffectiveDate = table.Column<DateTime>(nullable: false),
                    FajrAdhaan = table.Column<DateTime>(nullable: false),
                    FajrSalaah = table.Column<DateTime>(nullable: false),
                    IshaAdhaan = table.Column<DateTime>(nullable: false),
                    IshaSalaah = table.Column<DateTime>(nullable: false),
                    JumuahAdhaan = table.Column<DateTime>(nullable: false),
                    JumuahSalaah = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalaahTimeViewModel", x => x.KeyId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SalaahTimeViewModel");

            migrationBuilder.DropColumn(
                name: "TimeDate",
                table: "SalaahTime");
        }
    }
}
