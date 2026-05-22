using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentManagementApp.Migrations
{
    /// <inheritdoc />
    public partial class RemoveRoomDescription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Rooms");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Rooms",
                type: "text",
                nullable: true);
        }
    }
}
