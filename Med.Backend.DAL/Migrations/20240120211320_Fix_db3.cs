using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Med.Backend.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Fix_db3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Diagnos_Inspections_InspectionId",
                table: "Diagnos");

            migrationBuilder.AlterColumn<Guid>(
                name: "InspectionId",
                table: "Diagnos",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ConsultationId",
                table: "Consultations",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddForeignKey(
                name: "FK_Diagnos_Inspections_InspectionId",
                table: "Diagnos",
                column: "InspectionId",
                principalTable: "Inspections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Diagnos_Inspections_InspectionId",
                table: "Diagnos");

            migrationBuilder.DropColumn(
                name: "ConsultationId",
                table: "Consultations");

            migrationBuilder.AlterColumn<Guid>(
                name: "InspectionId",
                table: "Diagnos",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_Diagnos_Inspections_InspectionId",
                table: "Diagnos",
                column: "InspectionId",
                principalTable: "Inspections",
                principalColumn: "Id");
        }
    }
}
