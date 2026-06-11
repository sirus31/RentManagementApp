namespace RentManagementApp.DTOs.Responses
{
    public class BillFullDetailResponseDto
    {
        public int BillId { get; set; }


        public string BillNumber { get; set; }
            = string.Empty;


        public string TenantName { get; set; }
            = string.Empty;


        public string BillingMonth { get; set; }
            = string.Empty;


        public int BillingYear { get; set; }


        public decimal RentAmount { get; set; }


        public decimal ElectricityAmount { get; set; }


        public decimal GarbageAmount { get; set; }


        public decimal ExtraChargeAmount { get; set; }


        public decimal PreviousDueAmount { get; set; }


        public decimal TotalAmount { get; set; }


        public decimal AmountPaid { get; set; }


        public decimal PendingAmount { get; set; }


        public List<BillDetailResponseDto> Details { get; set; }
            = new();

        public List<string> Rooms { get; set; }
            = new();
    }
}