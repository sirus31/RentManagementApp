using RentManagementApp.Models.MasterEntities;
using RentManagementApp.Models.TransactionalEntities;

namespace RentManagementApp.Models.TransactionalEntities
{
    public class Bill
    {
        public int Id { get; set; }

        public string BillNumber { get; set; }

        public int TenantId { get; set; }

        public DateTime BillMonth { get; set; }

        public decimal RentAmount { get; set; }

        public decimal ElectricityAmount { get; set; }

        public decimal GarbageAmount { get; set; }

        public decimal TotalAmount { get; set; }

        public decimal AmountPaid { get; set; }

        public string PaymentStatus { get; set; }

        public string BillStatus { get; set; }

        public DateTime GeneratedDate { get; set; }

        public DateTime? FinalizedDate { get; set; }

        public Tenant Tenant { get; set; }

        public List<Payment> Payments { get; set; }
    }
}