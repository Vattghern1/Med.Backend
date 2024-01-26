using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Med.Backend.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Addicd10 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Icd10s",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ParentId = table.Column<int>(type: "integer", nullable: true),
                    ParentId1 = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Icd10s", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Icd10s_Icd10s_ParentId1",
                        column: x => x.ParentId1,
                        principalTable: "Icd10s",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Icd10s_ParentId1",
                table: "Icd10s",
                column: "ParentId1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Icd10s");
        }
    }
}
