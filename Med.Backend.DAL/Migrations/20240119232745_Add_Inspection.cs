using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Med.Backend.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Add_Inspection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ParentName",
                table: "Comment",
                newName: "ParentCommentId");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Specialities",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "InspectionId",
                table: "Consultations",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Inspections",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    InspectionDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Anamnesis = table.Column<string>(type: "text", nullable: false),
                    Treatment = table.Column<string>(type: "text", nullable: false),
                    Conclusion = table.Column<int>(type: "integer", nullable: false),
                    NextVisitDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeathDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    PreviousInspectionIdId = table.Column<Guid>(type: "uuid", nullable: true),
                    DoctorID = table.Column<Guid>(type: "uuid", nullable: false),
                    PatientId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inspections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Inspections_Inspections_PreviousInspectionIdId",
                        column: x => x.PreviousInspectionIdId,
                        principalTable: "Inspections",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Inspections_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Diagnos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IcdDiagnosisId = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    DiagnosisType = table.Column<int>(type: "integer", nullable: false),
                    InspectionId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Diagnos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Diagnos_Inspections_InspectionId",
                        column: x => x.InspectionId,
                        principalTable: "Inspections",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Specialities_UserId",
                table: "Specialities",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Consultations_InspectionId",
                table: "Consultations",
                column: "InspectionId");

            migrationBuilder.CreateIndex(
                name: "IX_Diagnos_InspectionId",
                table: "Diagnos",
                column: "InspectionId");

            migrationBuilder.CreateIndex(
                name: "IX_Inspections_PatientId",
                table: "Inspections",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_Inspections_PreviousInspectionIdId",
                table: "Inspections",
                column: "PreviousInspectionIdId");

            migrationBuilder.AddForeignKey(
                name: "FK_Consultations_Inspections_InspectionId",
                table: "Consultations",
                column: "InspectionId",
                principalTable: "Inspections",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Specialities_AspNetUsers_UserId",
                table: "Specialities",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Consultations_Inspections_InspectionId",
                table: "Consultations");

            migrationBuilder.DropForeignKey(
                name: "FK_Specialities_AspNetUsers_UserId",
                table: "Specialities");

            migrationBuilder.DropTable(
                name: "Diagnos");

            migrationBuilder.DropTable(
                name: "Inspections");

            migrationBuilder.DropIndex(
                name: "IX_Specialities_UserId",
                table: "Specialities");

            migrationBuilder.DropIndex(
                name: "IX_Consultations_InspectionId",
                table: "Consultations");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Specialities");

            migrationBuilder.DropColumn(
                name: "InspectionId",
                table: "Consultations");

            migrationBuilder.RenameColumn(
                name: "ParentCommentId",
                table: "Comment",
                newName: "ParentName");
        }
    }
}
