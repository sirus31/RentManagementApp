namespace RentManagementApp.DTOs.Responses
{
    public class OccupancyResponseDto
    {
        public int TenantRoomId { get; set; }

        public int TenantId { get; set; }

        public string TenantName { get; set; } = null!;

        public int RoomId { get; set; }

        public string RoomNumber { get; set; } = null!;

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}