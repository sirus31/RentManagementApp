namespace RentManagementApp.DTOs.Responses
{
    public class MeterOverviewResponseDto
    {
        public int Id { get; set; }


        public int HouseId { get; set; }


        public string MeterNumber { get; set; }
            = string.Empty;


        public string MeterType { get; set; }
            = string.Empty;


        public decimal InitialReading { get; set; }


        public bool IsActive { get; set; }


        public List<string> AssignedTenants { get; set; }
            = new();
    }
}