using RentManagementApp.Models.Enums;

namespace RentManagementApp.DTOs.Responses
{
    public class BillResponseDto
    {
        public int Id { get; set; }

        public string BillNumber { get; set; }
            = string.Empty;

        public string TenantName { get; set; }
            = string.Empty;

        public int BillingYear { get; set; }

        public BillingMonth BillingMonth { get; set; }

        public decimal RentAmount { get; set; }

        public decimal ElectricityAmount { get; set; }

        public decimal GarbageAmount { get; set; }

        public decimal TotalAmount { get; set; }

        public decimal AmountPaid { get; set; }

        public PaymentStatus PaymentStatus { get; set; }

        public BillStatus BillStatus { get; set; }

        public DateTime GeneratedDate { get; set; }
    
        public List<BillDetailResponseDto> BillDetails { get; set; }
            = new();
    }
}