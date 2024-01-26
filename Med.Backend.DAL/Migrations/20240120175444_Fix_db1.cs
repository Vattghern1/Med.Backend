using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Med.Backend.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Fix_db1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inspections_Inspections_PreviousInspectionIdId",
                table: "Inspections");

            migrationBuilder.DropForeignKey(
                name: "FK_Inspections_Patients_PatientId",
                table: "Inspections");

            migrationBuilder.DropIndex(
                name: "IX_Inspections_PreviousInspectionIdId",
                table: "Inspections");

            migrationBuilder.RenameColumn(
                name: "DoctorID",
                table: "Inspections",
                newName: "DoctorId");

            migrationBuilder.RenameColumn(
                name: "PreviousInspectionIdId",
                table: "Inspections",
                newName: "PreviousInspectionId");

            migrationBuilder.AlterColumn<Guid>(
                name: "PatientId",
                table: "Inspections",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DoctorName",
                table: "Inspections",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PatientName",
                table: "Inspections",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Inspections_Patients_PatientId",
                table: "Inspections",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inspections_Patients_PatientId",
                table: "Inspections");

            migrationBuilder.DropColumn(
                name: "DoctorName",
                table: "Inspections");

            migrationBuilder.DropColumn(
                name: "PatientName",
                table: "Inspections");

            migrationBuilder.RenameColumn(
                name: "DoctorId",
                table: "Inspections",
                newName: "DoctorID");

            migrationBuilder.RenameColumn(
                name: "PreviousInspectionId",
                table: "Inspections",
                newName: "PreviousInspectionIdId");

            migrationBuilder.AlterColumn<Guid>(
                name: "PatientId",
                table: "Inspections",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.CreateIndex(
                name: "IX_Inspections_PreviousInspectionIdId",
                table: "Inspections",
                column: "PreviousInspectionIdId");

            migrationBuilder.AddForeignKey(
                name: "FK_Inspections_Inspections_PreviousInspectionIdId",
                table: "Inspections",
                column: "PreviousInspectionIdId",
                principalTable: "Inspections",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Inspections_Patients_PatientId",
                table: "Inspections",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "Id");
        }
    }
}
