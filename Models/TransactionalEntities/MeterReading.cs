using RentManagementApp.Models.MasterEntities;
using RentManagementApp.Models.TransactionalEntities;

namespace RentManagementApp.Models.TransactionalEntities
{
    public class MeterReading
    {
        public int Id { get; set; }

        public int MeterId { get; set; }

        public decimal ReadingValue { get; set; }

        public DateTime ReadingDate { get; set; }

        public string Notes { get; set; }

        public Meter Meter { get; set; }
    }
}