namespace RentManagementApp.DTOs.Responses
{
    public class TenantWithRoomsResponseDto
    {
        public int Id { get; set; }

        public string FullName { get; set; } = null!;

        public List<string> RoomNumbers { get; set; } = new();
    }
}