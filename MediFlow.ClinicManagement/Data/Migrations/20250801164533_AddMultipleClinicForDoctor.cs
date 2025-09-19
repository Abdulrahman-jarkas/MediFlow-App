using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediFlow.UsersManagement.Migrations
{
    /// <inheritdoc />
    public partial class AddMultipleClinicForDoctor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Doctors_Clinics_ClinicId",
                schema: "UsersManagement",
                table: "Doctors");

            migrationBuilder.DropIndex(
                name: "IX_Doctors_ClinicId",
                schema: "UsersManagement",
                table: "Doctors");

            migrationBuilder.DropColumn(
                name: "ClinicId",
                schema: "UsersManagement",
                table: "Doctors");

            migrationBuilder.CreateTable(
                name: "ClinicDoctor",
                schema: "UsersManagement",
                columns: table => new
                {
                    ClinicsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DoctorsIdsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClinicDoctor", x => new { x.ClinicsId, x.DoctorsIdsId });
                    table.ForeignKey(
                        name: "FK_ClinicDoctor_Clinics_ClinicsId",
                        column: x => x.ClinicsId,
                        principalSchema: "UsersManagement",
                        principalTable: "Clinics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClinicDoctor_Doctors_DoctorsIdsId",
                        column: x => x.DoctorsIdsId,
                        principalSchema: "UsersManagement",
                        principalTable: "Doctors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClinicDoctor_DoctorsIdsId",
                schema: "UsersManagement",
                table: "ClinicDoctor",
                column: "DoctorsIdsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClinicDoctor",
                schema: "UsersManagement");

            migrationBuilder.AddColumn<Guid>(
                name: "ClinicId",
                schema: "UsersManagement",
                table: "Doctors",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Doctors_ClinicId",
                schema: "UsersManagement",
                table: "Doctors",
                column: "ClinicId");

            migrationBuilder.AddForeignKey(
                name: "FK_Doctors_Clinics_ClinicId",
                schema: "UsersManagement",
                table: "Doctors",
                column: "ClinicId",
                principalSchema: "UsersManagement",
                principalTable: "Clinics",
                principalColumn: "Id");
        }
    }
}
