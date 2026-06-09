using RentManagementApp.Models.Enums;
using RentManagementApp.Models.MasterEntities;


namespace RentManagementApp.Models.TransactionalEntities
{
    public class BillCycle
    {
        public int Id { get; set; }


        public int HouseId { get; set; }


        public BillingMonth BillingMonth { get; set; }


        public int BillingYear { get; set; }


        public BillCycleType CycleType { get; set; }


        public BillStatus Status { get; set; }


        public DateTime GeneratedDate { get; set; }


        public DateTime? FinalizedDate { get; set; }


        public House House { get; set; }
            = null!;


        public List<Bill> Bills { get; set; }
            = new();
    }
}