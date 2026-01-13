using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TransportationAttendance.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePhase1Schema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentBusAssignments_Buses_ArrivalBusId",
                table: "StudentBusAssignments");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentBusAssignments_Buses_BusId",
                table: "StudentBusAssignments");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentBusAssignments_Buses_ReturnBusId",
                table: "StudentBusAssignments");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentBusAssignments_Students_StudentId",
                table: "StudentBusAssignments");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "StudentBusAssignments",
                newName: "StudentBusAssignmentId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentBusAssignments_IsActive",
                table: "StudentBusAssignments",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_StudentBusAssignments_IsDeleted",
                table: "StudentBusAssignments",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_StudentBusAssignments_StudentId_BusId",
                table: "StudentBusAssignments",
                columns: new[] { "StudentId", "BusId" },
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentBusAssignments_Buses_ArrivalBusId",
                table: "StudentBusAssignments",
                column: "ArrivalBusId",
                principalTable: "Buses",
                principalColumn: "BusId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentBusAssignments_Buses_BusId",
                table: "StudentBusAssignments",
                column: "BusId",
                principalTable: "Buses",
                principalColumn: "BusId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentBusAssignments_Buses_ReturnBusId",
                table: "StudentBusAssignments",
                column: "ReturnBusId",
                principalTable: "Buses",
                principalColumn: "BusId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentBusAssignments_Students_StudentId",
                table: "StudentBusAssignments",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "StudentId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentBusAssignments_Buses_ArrivalBusId",
                table: "StudentBusAssignments");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentBusAssignments_Buses_BusId",
                table: "StudentBusAssignments");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentBusAssignments_Buses_ReturnBusId",
                table: "StudentBusAssignments");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentBusAssignments_Students_StudentId",
                table: "StudentBusAssignments");

            migrationBuilder.DropIndex(
                name: "IX_StudentBusAssignments_IsActive",
                table: "StudentBusAssignments");

            migrationBuilder.DropIndex(
                name: "IX_StudentBusAssignments_IsDeleted",
                table: "StudentBusAssignments");

            migrationBuilder.DropIndex(
                name: "IX_StudentBusAssignments_StudentId_BusId",
                table: "StudentBusAssignments");

            migrationBuilder.RenameColumn(
                name: "StudentBusAssignmentId",
                table: "StudentBusAssignments",
                newName: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentBusAssignments_Buses_ArrivalBusId",
                table: "StudentBusAssignments",
                column: "ArrivalBusId",
                principalTable: "Buses",
                principalColumn: "BusId");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentBusAssignments_Buses_BusId",
                table: "StudentBusAssignments",
                column: "BusId",
                principalTable: "Buses",
                principalColumn: "BusId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentBusAssignments_Buses_ReturnBusId",
                table: "StudentBusAssignments",
                column: "ReturnBusId",
                principalTable: "Buses",
                principalColumn: "BusId");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentBusAssignments_Students_StudentId",
                table: "StudentBusAssignments",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "StudentId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
