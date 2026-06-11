using RentManagementApp.Models.Enums;


namespace RentManagementApp.DTOs.Requests
{
    public class GenerateMonthlyBillRequestDto
    {
        public int HouseId { get; set; }


        public int BillingYear { get; set; }


        public BillingMonth BillingMonth { get; set; }



        public List<GenerateMeterReadingDto> MeterReadings { get; set; }
            = new();



        public List<GenerateExtraChargeDto> ExtraCharges { get; set; }
            = new();
    }



    public class GenerateMeterReadingDto
    {
        public int MeterId { get; set; }


        public decimal ReadingValue { get; set; }
    }



    public class GenerateExtraChargeDto
    {
        public string ChargeName { get; set; }
            = string.Empty;


        public decimal Amount { get; set; }


        public List<int> TenantIds { get; set; }
            = new();
    }
}