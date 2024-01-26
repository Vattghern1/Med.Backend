using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Med.Backend.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Fix_db5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_Consultations_ConsultationId",
                table: "Comment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Comment",
                table: "Comment");

            migrationBuilder.RenameTable(
                name: "Comment",
                newName: "Comments");

            migrationBuilder.RenameIndex(
                name: "IX_Comment_ConsultationId",
                table: "Comments",
                newName: "IX_Comments_ConsultationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Comments",
                table: "Comments",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Consultations_ConsultationId",
                table: "Comments",
                column: "ConsultationId",
                principalTable: "Consultations",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Consultations_ConsultationId",
                table: "Comments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Comments",
                table: "Comments");

            migrationBuilder.RenameTable(
                name: "Comments",
                newName: "Comment");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_ConsultationId",
                table: "Comment",
                newName: "IX_Comment_ConsultationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Comment",
                table: "Comment",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_Consultations_ConsultationId",
                table: "Comment",
                column: "ConsultationId",
                principalTable: "Consultations",
                principalColumn: "Id");
        }
    }
}
