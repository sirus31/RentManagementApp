namespace RentManagementApp.DTOs.Responses
{
    public class TenantRoomSummaryResponseDto
    {
        public int Id { get; set; }

        public string FullName { get; set; } = null!;

        public List<string> RoomNumbers { get; set; } = new();
    }
}