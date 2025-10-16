using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NewParkingApp.Migrations
{
    /// <inheritdoc />
    public partial class initialCreate3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Periods_CarId",
                table: "Periods");

            migrationBuilder.DropIndex(
                name: "IX_Periods_CarId1",
                table: "Periods");

            migrationBuilder.CreateIndex(
                name: "IX_Periods_CarId",
                table: "Periods",
                column: "CarId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Periods_CarId1",
                table: "Periods",
                column: "CarId1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Periods_CarId",
                table: "Periods");

            migrationBuilder.DropIndex(
                name: "IX_Periods_CarId1",
                table: "Periods");

            migrationBuilder.CreateIndex(
                name: "IX_Periods_CarId",
                table: "Periods",
                column: "CarId");

            migrationBuilder.CreateIndex(
                name: "IX_Periods_CarId1",
                table: "Periods",
                column: "CarId1",
                unique: true);
        }
    }
}
