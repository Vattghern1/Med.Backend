using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Med.Backend.DAL.Migrations
{
    /// <inheritdoc />
    public partial class MigrateDB1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Speciality_AspNetUsers_UserId",
                table: "Speciality");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Speciality",
                table: "Speciality");

            migrationBuilder.DropIndex(
                name: "IX_Speciality_UserId",
                table: "Speciality");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Speciality");

            migrationBuilder.RenameTable(
                name: "Speciality",
                newName: "Specialities");

            migrationBuilder.AddColumn<List<Guid>>(
                name: "UserSpecialities",
                table: "AspNetUsers",
                type: "uuid[]",
                nullable: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Specialities",
                table: "Specialities",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Specialities",
                table: "Specialities");

            migrationBuilder.DropColumn(
                name: "UserSpecialities",
                table: "AspNetUsers");

            migrationBuilder.RenameTable(
                name: "Specialities",
                newName: "Speciality");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Speciality",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Speciality",
                table: "Speciality",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Speciality_UserId",
                table: "Speciality",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Speciality_AspNetUsers_UserId",
                table: "Speciality",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
