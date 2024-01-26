using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Med.Backend.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Addicd103 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Icd10s_Icd10s_ParentId1",
                table: "Icd10s");

            migrationBuilder.DropColumn(
                name: "CreateTime",
                table: "Icd10s");

            migrationBuilder.RenameColumn(
                name: "ParentId1",
                table: "Icd10s",
                newName: "Icd10Id");

            migrationBuilder.RenameIndex(
                name: "IX_Icd10s_ParentId1",
                table: "Icd10s",
                newName: "IX_Icd10s_Icd10Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Icd10s_Icd10s_Icd10Id",
                table: "Icd10s",
                column: "Icd10Id",
                principalTable: "Icd10s",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Icd10s_Icd10s_Icd10Id",
                table: "Icd10s");

            migrationBuilder.RenameColumn(
                name: "Icd10Id",
                table: "Icd10s",
                newName: "ParentId1");

            migrationBuilder.RenameIndex(
                name: "IX_Icd10s_Icd10Id",
                table: "Icd10s",
                newName: "IX_Icd10s_ParentId1");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateTime",
                table: "Icd10s",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddForeignKey(
                name: "FK_Icd10s_Icd10s_ParentId1",
                table: "Icd10s",
                column: "ParentId1",
                principalTable: "Icd10s",
                principalColumn: "Id");
        }
    }
}
