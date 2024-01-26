using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Med.Backend.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Fix_mirgation2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreateTime",
                table: "Diagnos",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "DiagnosName",
                table: "Diagnos",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreateTime",
                table: "Diagnos");

            migrationBuilder.DropColumn(
                name: "DiagnosName",
                table: "Diagnos");
        }
    }
}
