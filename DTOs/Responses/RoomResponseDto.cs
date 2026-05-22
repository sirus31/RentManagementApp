namespace RentManagementApp.DTOs.Responses
{
    public class RoomResponseDto
    {
        public int Id { get; set; }

        public int FloorId { get; set; }

        public string RoomNumber { get; set; } = null!;
    }
}