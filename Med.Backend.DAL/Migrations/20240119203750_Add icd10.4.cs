using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Med.Backend.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Addicd104 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Icd10s_Icd10s_Icd10Id",
                table: "Icd10s");

            migrationBuilder.DropIndex(
                name: "IX_Icd10s_Icd10Id",
                table: "Icd10s");

            migrationBuilder.DropColumn(
                name: "Icd10Id",
                table: "Icd10s");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "Icd10Id",
                table: "Icd10s",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Icd10s_Icd10Id",
                table: "Icd10s",
                column: "Icd10Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Icd10s_Icd10s_Icd10Id",
                table: "Icd10s",
                column: "Icd10Id",
                principalTable: "Icd10s",
                principalColumn: "Id");
        }
    }
}
