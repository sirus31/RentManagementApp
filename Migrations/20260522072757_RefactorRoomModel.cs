using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentManagementApp.Migrations
{
    /// <inheritdoc />
    public partial class RefactorRoomModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsOccupied",
                table: "Rooms");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Rooms",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Rooms");

            migrationBuilder.AddColumn<bool>(
                name: "IsOccupied",
                table: "Rooms",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
