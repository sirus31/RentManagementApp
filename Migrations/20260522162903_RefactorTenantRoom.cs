using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentManagementApp.Migrations
{
    /// <inheritdoc />
    public partial class RefactorTenantRoom : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "TenantRooms");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "TenantRooms",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
