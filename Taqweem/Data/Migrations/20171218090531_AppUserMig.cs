using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Taqweem.Data.Migrations
{
    public partial class AppUserMig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notice_AspNetUsers_CreatedById",
                table: "Notice");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "CreatedById",
                table: "Notice",
                newName: "CreatedId");

            migrationBuilder.RenameIndex(
                name: "IX_Notice_CreatedById",
                table: "Notice",
                newName: "IX_Notice_CreatedId");

            migrationBuilder.RenameColumn(
                name: "UpdatedOn",
                table: "AspNetUsers",
                newName: "CreatedAt");

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Notice",
                maxLength: 38,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ActiveStatus",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "AspNetUsers",
                maxLength: 38,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Notice_AspNetUsers_CreatedId",
                table: "Notice",
                column: "CreatedId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notice_AspNetUsers_CreatedId",
                table: "Notice");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Notice");

            migrationBuilder.DropColumn(
                name: "ActiveStatus",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "CreatedId",
                table: "Notice",
                newName: "CreatedById");

            migrationBuilder.RenameIndex(
                name: "IX_Notice_CreatedId",
                table: "Notice",
                newName: "IX_Notice_CreatedById");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "AspNetUsers",
                newName: "UpdatedOn");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddForeignKey(
                name: "FK_Notice_AspNetUsers_CreatedById",
                table: "Notice",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
