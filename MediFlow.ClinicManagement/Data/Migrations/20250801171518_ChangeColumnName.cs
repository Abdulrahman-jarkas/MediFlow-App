using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediFlow.UsersManagement.Migrations
{
    /// <inheritdoc />
    public partial class ChangeColumnName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClinicDoctor_Doctors_DoctorsIdsId",
                schema: "UsersManagement",
                table: "ClinicDoctor");

            migrationBuilder.RenameColumn(
                name: "DoctorsIdsId",
                schema: "UsersManagement",
                table: "ClinicDoctor",
                newName: "DoctorsId");

            migrationBuilder.RenameIndex(
                name: "IX_ClinicDoctor_DoctorsIdsId",
                schema: "UsersManagement",
                table: "ClinicDoctor",
                newName: "IX_ClinicDoctor_DoctorsId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClinicDoctor_Doctors_DoctorsId",
                schema: "UsersManagement",
                table: "ClinicDoctor",
                column: "DoctorsId",
                principalSchema: "UsersManagement",
                principalTable: "Doctors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClinicDoctor_Doctors_DoctorsId",
                schema: "UsersManagement",
                table: "ClinicDoctor");

            migrationBuilder.RenameColumn(
                name: "DoctorsId",
                schema: "UsersManagement",
                table: "ClinicDoctor",
                newName: "DoctorsIdsId");

            migrationBuilder.RenameIndex(
                name: "IX_ClinicDoctor_DoctorsId",
                schema: "UsersManagement",
                table: "ClinicDoctor",
                newName: "IX_ClinicDoctor_DoctorsIdsId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClinicDoctor_Doctors_DoctorsIdsId",
                schema: "UsersManagement",
                table: "ClinicDoctor",
                column: "DoctorsIdsId",
                principalSchema: "UsersManagement",
                principalTable: "Doctors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
