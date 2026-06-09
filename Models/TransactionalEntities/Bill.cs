using RentManagementApp.Models.Enums;
using RentManagementApp.Models.MasterEntities;


namespace RentManagementApp.Models.TransactionalEntities
{
    public class Bill
    {
        public int Id { get; set; }


        public string BillNumber { get; set; }
            = string.Empty;


        public int BillCycleId { get; set; }


        public int TenantId { get; set; }



        public decimal RentAmount { get; set; }


        public decimal ElectricityAmount { get; set; }


        public decimal GarbageAmount { get; set; }


        public decimal ExtraChargeAmount { get; set; }


        public decimal PreviousDueAmount { get; set; }


        public decimal TotalAmount { get; set; }


        public decimal AmountPaid { get; set; }


        public PaymentStatus PaymentStatus { get; set; }



        public BillCycle BillCycle { get; set; }
            = null!;


        public Tenant Tenant { get; set; }
            = null!;


        public List<Payment> Payments { get; set; }
            = new();


        public List<BillDetail> BillDetails { get; set; }
            = new();
    }
}