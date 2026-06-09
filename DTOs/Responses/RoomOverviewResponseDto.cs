namespace RentManagementApp.DTOs.Responses
{
    public class RoomOverviewResponseDto
    {
        public int RoomId { get; set; }


        public string RoomNumber { get; set; } = null!;


        public bool IsOccupied { get; set; }


        public string? TenantName { get; set; }
    }
}