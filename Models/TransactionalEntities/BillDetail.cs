using RentManagementApp.Models.Enums;

namespace RentManagementApp.Models.TransactionalEntities
{
    public class BillDetail
    {
        public int Id { get; set; }


        public int BillId { get; set; }


        public BillDetailType DetailType { get; set; }


        public string Description { get; set; }
            = string.Empty;


        public decimal PreviousReading { get; set; }


        public decimal CurrentReading { get; set; }


        public decimal UnitsConsumed { get; set; }


        public decimal Rate { get; set; }


        public decimal Amount { get; set; }


        public Bill Bill { get; set; }
            = null!;
    }
}