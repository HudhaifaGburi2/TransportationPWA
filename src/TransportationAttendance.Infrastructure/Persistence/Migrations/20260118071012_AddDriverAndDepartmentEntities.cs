using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TransportationAttendance.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddDriverAndDepartmentEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BusNumber",
                table: "Buses",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DepartmentId",
                table: "Buses",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DriverId",
                table: "Buses",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    DepartmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.DepartmentId);
                });

            migrationBuilder.CreateTable(
                name: "Drivers",
                columns: table => new
                {
                    DriverId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    DepartmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Drivers", x => x.DriverId);
                    table.ForeignKey(
                        name: "FK_Drivers_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "DepartmentId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Buses_DepartmentId",
                table: "Buses",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Buses_DriverId",
                table: "Buses",
                column: "DriverId");

            migrationBuilder.CreateIndex(
                name: "IX_Buses_PlateNumber_PeriodId",
                table: "Buses",
                columns: new[] { "PlateNumber", "PeriodId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Departments_Code",
                table: "Departments",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Departments_IsActive",
                table: "Departments",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_IsDeleted",
                table: "Departments",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Drivers_DepartmentId",
                table: "Drivers",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Drivers_IsActive",
                table: "Drivers",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Drivers_IsDeleted",
                table: "Drivers",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Drivers_PhoneNumber",
                table: "Drivers",
                column: "PhoneNumber");

            migrationBuilder.AddForeignKey(
                name: "FK_Buses_Departments_DepartmentId",
                table: "Buses",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "DepartmentId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Buses_Drivers_DriverId",
                table: "Buses",
                column: "DriverId",
                principalTable: "Drivers",
                principalColumn: "DriverId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Buses_Departments_DepartmentId",
                table: "Buses");

            migrationBuilder.DropForeignKey(
                name: "FK_Buses_Drivers_DriverId",
                table: "Buses");

            migrationBuilder.DropTable(
                name: "Drivers");

            migrationBuilder.DropTable(
                name: "Departments");

            migrationBuilder.DropIndex(
                name: "IX_Buses_DepartmentId",
                table: "Buses");

            migrationBuilder.DropIndex(
                name: "IX_Buses_DriverId",
                table: "Buses");

            migrationBuilder.DropIndex(
                name: "IX_Buses_PlateNumber_PeriodId",
                table: "Buses");

            migrationBuilder.DropColumn(
                name: "BusNumber",
                table: "Buses");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "Buses");

            migrationBuilder.DropColumn(
                name: "DriverId",
                table: "Buses");
        }
    }
}
