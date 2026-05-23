using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentManagementApp.Migrations
{
    /// <inheritdoc />
    public partial class RefactorMeterAndTenantMeter : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "TenantMeters");

            migrationBuilder.RenameColumn(
                name: "MeterName",
                table: "Meters",
                newName: "MeterNumber");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "TenantMeters",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "TenantMeters",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "TenantMeters");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "TenantMeters");

            migrationBuilder.RenameColumn(
                name: "MeterNumber",
                table: "Meters",
                newName: "MeterName");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "TenantMeters",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
