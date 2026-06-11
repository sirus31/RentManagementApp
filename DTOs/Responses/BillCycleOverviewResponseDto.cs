using RentManagementApp.Models.Enums;


namespace RentManagementApp.DTOs.Responses
{
    public class BillCycleOverviewResponseDto
    {
        public int Id { get; set; }
        public int BillingYear { get; set; }
        public BillingMonth BillingMonth { get; set; }
        public BillCycleType CycleType { get; set; }
        public BillStatus Status { get; set; }
        public DateTime GeneratedDate { get; set; }
        // CARD METADATA
        public int TotalBills { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal PendingAmount { get; set; }
        public int PaidCount { get; set; }
        public int PartialCount { get; set; }
        public int PendingCount { get; set; }
        // EXPANDED TABLE DATA
        public List<BillCycleTenantBillDto> Bills
        { get; set; } = new();
    }

    public class BillCycleTenantBillDto
    {
        public int BillId { get; set; }
        public string TenantName { get; set; }
            = string.Empty;
        public List<string> Rooms { get; set; }
            = new();
        public decimal RentAmount { get; set; }
        public decimal ElectricityAmount { get; set; }
        public decimal GarbageAmount { get; set; }
        public decimal ExtraChargeAmount { get; set; }
        public decimal PreviousDueAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal AmountPaid { get; set; }
        public decimal PendingAmount { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
    }
}