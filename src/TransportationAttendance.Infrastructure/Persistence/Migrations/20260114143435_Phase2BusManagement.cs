using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TransportationAttendance.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Phase2BusManagement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "BusId1",
                table: "StudentBusAssignments",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BusDistricts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BusId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DistrictId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    table.PrimaryKey("PK_BusDistricts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BusDistricts_Buses_BusId",
                        column: x => x.BusId,
                        principalTable: "Buses",
                        principalColumn: "BusId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BusDistricts_Districts_DistrictId",
                        column: x => x.DistrictId,
                        principalTable: "Districts",
                        principalColumn: "DistrictId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StudentBusAssignments_BusId1",
                table: "StudentBusAssignments",
                column: "BusId1");

            migrationBuilder.CreateIndex(
                name: "IX_BusDistricts_BusId_DistrictId",
                table: "BusDistricts",
                columns: new[] { "BusId", "DistrictId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BusDistricts_DistrictId",
                table: "BusDistricts",
                column: "DistrictId");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentBusAssignments_Buses_BusId1",
                table: "StudentBusAssignments",
                column: "BusId1",
                principalTable: "Buses",
                principalColumn: "BusId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentBusAssignments_Buses_BusId1",
                table: "StudentBusAssignments");

            migrationBuilder.DropTable(
                name: "BusDistricts");

            migrationBuilder.DropIndex(
                name: "IX_StudentBusAssignments_BusId1",
                table: "StudentBusAssignments");

            migrationBuilder.DropColumn(
                name: "BusId1",
                table: "StudentBusAssignments");
        }
    }
}
