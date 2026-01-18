using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TransportationAttendance.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddPeriodsAndFullNationalAddress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BusNumber",
                table: "Buses",
                newName: "PlateNumber");

            migrationBuilder.RenameIndex(
                name: "IX_Buses_BusNumber",
                table: "Buses",
                newName: "IX_Buses_PlateNumber");

            migrationBuilder.AddColumn<string>(
                name: "FullNationalAddress",
                table: "RegistrationRequests",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Periods",
                table: "RegistrationRequests",
                type: "NVARCHAR(MAX)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FullNationalAddress",
                table: "RegistrationRequests");

            migrationBuilder.DropColumn(
                name: "Periods",
                table: "RegistrationRequests");

            migrationBuilder.RenameColumn(
                name: "PlateNumber",
                table: "Buses",
                newName: "BusNumber");

            migrationBuilder.RenameIndex(
                name: "IX_Buses_PlateNumber",
                table: "Buses",
                newName: "IX_Buses_BusNumber");
        }
    }
}
