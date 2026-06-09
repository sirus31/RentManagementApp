using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace RentManagementApp.Migrations
{
    /// <inheritdoc />
    public partial class AddBillCycleArchitecture : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BillStatus",
                table: "Bills");

            migrationBuilder.DropColumn(
                name: "BillingMonth",
                table: "Bills");

            migrationBuilder.DropColumn(
                name: "FinalizedDate",
                table: "Bills");

            migrationBuilder.DropColumn(
                name: "GeneratedDate",
                table: "Bills");

            migrationBuilder.RenameColumn(
                name: "BillingYear",
                table: "Bills",
                newName: "BillCycleId");

            migrationBuilder.AddColumn<decimal>(
                name: "ExtraChargeAmount",
                table: "Bills",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "PreviousDueAmount",
                table: "Bills",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "MeterId",
                table: "BillDetails",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SharedTenantCount",
                table: "BillDetails",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "TenantUnits",
                table: "BillDetails",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "BillCycles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HouseId = table.Column<int>(type: "integer", nullable: false),
                    BillingMonth = table.Column<int>(type: "integer", nullable: false),
                    BillingYear = table.Column<int>(type: "integer", nullable: false),
                    CycleType = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    GeneratedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FinalizedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BillCycles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BillCycles_Houses_HouseId",
                        column: x => x.HouseId,
                        principalTable: "Houses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bills_BillCycleId",
                table: "Bills",
                column: "BillCycleId");

            migrationBuilder.CreateIndex(
                name: "IX_BillDetails_MeterId",
                table: "BillDetails",
                column: "MeterId");

            migrationBuilder.CreateIndex(
                name: "IX_BillCycles_HouseId",
                table: "BillCycles",
                column: "HouseId");

            migrationBuilder.AddForeignKey(
                name: "FK_BillDetails_Meters_MeterId",
                table: "BillDetails",
                column: "MeterId",
                principalTable: "Meters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Bills_BillCycles_BillCycleId",
                table: "Bills",
                column: "BillCycleId",
                principalTable: "BillCycles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BillDetails_Meters_MeterId",
                table: "BillDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_Bills_BillCycles_BillCycleId",
                table: "Bills");

            migrationBuilder.DropTable(
                name: "BillCycles");

            migrationBuilder.DropIndex(
                name: "IX_Bills_BillCycleId",
                table: "Bills");

            migrationBuilder.DropIndex(
                name: "IX_BillDetails_MeterId",
                table: "BillDetails");

            migrationBuilder.DropColumn(
                name: "ExtraChargeAmount",
                table: "Bills");

            migrationBuilder.DropColumn(
                name: "PreviousDueAmount",
                table: "Bills");

            migrationBuilder.DropColumn(
                name: "MeterId",
                table: "BillDetails");

            migrationBuilder.DropColumn(
                name: "SharedTenantCount",
                table: "BillDetails");

            migrationBuilder.DropColumn(
                name: "TenantUnits",
                table: "BillDetails");

            migrationBuilder.RenameColumn(
                name: "BillCycleId",
                table: "Bills",
                newName: "BillingYear");

            migrationBuilder.AddColumn<int>(
                name: "BillStatus",
                table: "Bills",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BillingMonth",
                table: "Bills",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "FinalizedDate",
                table: "Bills",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "GeneratedDate",
                table: "Bills",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
