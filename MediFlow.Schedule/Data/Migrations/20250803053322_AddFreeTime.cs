using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediFlow.Schedule.Migrations
{
    /// <inheritdoc />
    public partial class AddFreeTime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Doctors_Clinics_ClinicId",
                schema: "Schedule",
                table: "Doctors");

            migrationBuilder.DropIndex(
                name: "IX_Doctors_ClinicId",
                schema: "Schedule",
                table: "Doctors");

            migrationBuilder.DropColumn(
                name: "ClinicId",
                schema: "Schedule",
                table: "Doctors");

            migrationBuilder.CreateTable(
                name: "ClinicDoctor",
                schema: "Schedule",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClinicId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DoctorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AvailabilityData = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClinicDoctor", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClinicDoctor_Clinics_ClinicId",
                        column: x => x.ClinicId,
                        principalSchema: "Schedule",
                        principalTable: "Clinics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClinicDoctor_Doctors_DoctorId",
                        column: x => x.DoctorId,
                        principalSchema: "Schedule",
                        principalTable: "Doctors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClinicDoctor_ClinicId",
                schema: "Schedule",
                table: "ClinicDoctor",
                column: "ClinicId");

            migrationBuilder.CreateIndex(
                name: "IX_ClinicDoctor_DoctorId",
                schema: "Schedule",
                table: "ClinicDoctor",
                column: "DoctorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClinicDoctor",
                schema: "Schedule");

            migrationBuilder.AddColumn<Guid>(
                name: "ClinicId",
                schema: "Schedule",
                table: "Doctors",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Doctors_ClinicId",
                schema: "Schedule",
                table: "Doctors",
                column: "ClinicId");

            migrationBuilder.AddForeignKey(
                name: "FK_Doctors_Clinics_ClinicId",
                schema: "Schedule",
                table: "Doctors",
                column: "ClinicId",
                principalSchema: "Schedule",
                principalTable: "Clinics",
                principalColumn: "Id");
        }
    }
}
