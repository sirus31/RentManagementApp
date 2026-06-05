using RentManagementApp.Models.MasterEntities;
using RentManagementApp.Models.TransactionalEntities;

namespace RentManagementApp.Models.TransactionalEntities
{
    public class Payment
    {
        public int Id { get; set; }

        public int BillId { get; set; }

        public decimal Amount { get; set; }

        public DateTime PaymentDate { get; set; }

        public string PaymentMode { get; set; }
            = string.Empty;

        public string Notes { get; set; }
            = string.Empty;

        public Bill Bill { get; set; }
            = null!;
    }
}