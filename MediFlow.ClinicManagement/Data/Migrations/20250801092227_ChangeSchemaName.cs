using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediFlow.UsersManagement.Migrations
{
    /// <inheritdoc />
    public partial class ChangeSchemaName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "UsersManagement");

            migrationBuilder.RenameTable(
                name: "Rooms",
                schema: "ClinicManagement",
                newName: "Rooms",
                newSchema: "UsersManagement");

            migrationBuilder.RenameTable(
                name: "Medicines",
                schema: "ClinicManagement",
                newName: "Medicines",
                newSchema: "UsersManagement");

            migrationBuilder.RenameTable(
                name: "Equipments",
                schema: "ClinicManagement",
                newName: "Equipments",
                newSchema: "UsersManagement");

            migrationBuilder.RenameTable(
                name: "Doctors",
                schema: "ClinicManagement",
                newName: "Doctors",
                newSchema: "UsersManagement");

            migrationBuilder.RenameTable(
                name: "Clinics",
                schema: "ClinicManagement",
                newName: "Clinics",
                newSchema: "UsersManagement");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "ClinicManagement");

            migrationBuilder.RenameTable(
                name: "Rooms",
                schema: "UsersManagement",
                newName: "Rooms",
                newSchema: "ClinicManagement");

            migrationBuilder.RenameTable(
                name: "Medicines",
                schema: "UsersManagement",
                newName: "Medicines",
                newSchema: "ClinicManagement");

            migrationBuilder.RenameTable(
                name: "Equipments",
                schema: "UsersManagement",
                newName: "Equipments",
                newSchema: "ClinicManagement");

            migrationBuilder.RenameTable(
                name: "Doctors",
                schema: "UsersManagement",
                newName: "Doctors",
                newSchema: "ClinicManagement");

            migrationBuilder.RenameTable(
                name: "Clinics",
                schema: "UsersManagement",
                newName: "Clinics",
                newSchema: "ClinicManagement");
        }
    }
}
