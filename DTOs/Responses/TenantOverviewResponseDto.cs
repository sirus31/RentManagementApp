namespace RentManagementApp.DTOs.Responses
{
    public class TenantOverviewResponseDto
    {
        public int TenantId { get; set; }


        public string TenantName { get; set; } = string.Empty;


        public string PhoneNumber { get; set; } = string.Empty;


        public decimal MonthlyRent { get; set; }


        public bool IsActive { get; set; }


        public List<string> Rooms { get; set; } = new();


        public List<string> Meters { get; set; } = new();
    }
}