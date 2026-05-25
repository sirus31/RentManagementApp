using RentManagementApp.Models.Enums;

namespace RentManagementApp.DTOs.Requests
{
    public class GenerateBillRequestDto
    {
        public int TenantId { get; set; }

        public int BillingYear { get; set; }

        public BillingMonth BillingMonth { get; set; }
    }
}