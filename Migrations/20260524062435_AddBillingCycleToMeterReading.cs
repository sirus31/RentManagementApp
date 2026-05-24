using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentManagementApp.Migrations
{
    /// <inheritdoc />
    public partial class AddBillingCycleToMeterReading : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "MeterReadings",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<int>(
                name: "BillingMonth",
                table: "MeterReadings",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BillingYear",
                table: "MeterReadings",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BillingMonth",
                table: "MeterReadings");

            migrationBuilder.DropColumn(
                name: "BillingYear",
                table: "MeterReadings");

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "MeterReadings",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }
    }
}
