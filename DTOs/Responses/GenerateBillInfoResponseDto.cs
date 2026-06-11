namespace RentManagementApp.DTOs.Responses
{
    public class GenerateBillInfoResponseDto
    {
        public GenerateBillHouseDto House { get; set; }
            = new();


        public List<GenerateBillTenantDto> Tenants { get; set; }
            = new();


        public List<GenerateBillMeterDto> Meters { get; set; }
            = new();


        public List<GenerateBillPreviousDueDto> PreviousDues { get; set; }
            = new();
    }



    public class GenerateBillHouseDto
    {
        public int Id { get; set; }


        public string Name { get; set; }
            = string.Empty;


        public string Address { get; set; }
            = string.Empty;


        public decimal ElectricityRate { get; set; }


        public decimal GarbageFee { get; set; }
    }





    public class GenerateBillTenantDto
    {
        public int TenantId { get; set; }


        public string TenantName { get; set; }
            = string.Empty;


        public List<string> Rooms { get; set; }
            = new();


        public decimal MonthlyRent { get; set; }


        public List<string> Meters { get; set; }
            = new();
    }





    public class GenerateBillMeterDto
    {
        public int MeterId { get; set; }


        public string MeterNumber { get; set; }
            = string.Empty;


        public string MeterType { get; set; }
            = string.Empty;


        public decimal PreviousReading { get; set; }
    }






    public class GenerateBillPreviousDueDto
    {
        public int TenantId { get; set; }


        public string TenantName { get; set; }
            = string.Empty;


        public string BillingMonth { get; set; }
            = string.Empty;


        public int BillingYear { get; set; }


        public decimal DueAmount { get; set; }
    }
}