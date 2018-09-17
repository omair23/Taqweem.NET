using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Taqweem.Data.Migrations
{
    public partial class Currencies : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "World");

            migrationBuilder.CreateTable(
                name: "Currency",
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
                    Code = table.Column<string>(nullable: false),
                    Style = table.Column<int>(nullable: false),
                    Symbol = table.Column<string>(nullable: true),
                    Flag = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: false),
                    NumberToBasic = table.Column<int>(nullable: false),
                    FractionalUnit = table.Column<string>(nullable: true),
                    Locations = table.Column<string>(nullable: true),
                    ConversionRate = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currency", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Currency",
                schema: "World");
        }
    }
}
