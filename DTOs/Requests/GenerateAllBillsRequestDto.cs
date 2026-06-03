using RentManagementApp.Models.Enums;


namespace RentManagementApp.DTOs.Requests
{
    public class GenerateAllBillsRequestDto
    {
        public int BillingYear { get; set; }


        public BillingMonth BillingMonth { get; set; }
    }
}