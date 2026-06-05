using RentManagementApp.Models.Enums;
using RentManagementApp.Models.MasterEntities;
using RentManagementApp.Models.TransactionalEntities;

namespace RentManagementApp.Models.TransactionalEntities
{
    public class Bill
    {
        public int Id { get; set; }

        public string BillNumber { get; set; }
            = string.Empty;

        public int TenantId { get; set; }

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

        public DateTime? FinalizedDate { get; set; }

        public Tenant Tenant { get; set; }
            = null!;

        public List<Payment> Payments { get; set; }
            = new();

        public List<BillDetail> BillDetails { get; set; }
            = new();
    }
}