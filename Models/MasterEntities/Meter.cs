using RentManagementApp.Models.RelationshipEntities;
using RentManagementApp.Models.TransactionalEntities;
using RentManagementApp.Models.Enums;

namespace RentManagementApp.Models.MasterEntities
{
    public class Meter
    {
        public int Id { get; set; }

        public int HouseId { get; set; }

        public string MeterName { get; set; }

        public MeterType MeterType { get; set; }

        public decimal InitialReading { get; set; }

        public bool IsActive { get; set; }

        public House House { get; set; }

        public List<TenantMeter> TenantMeters { get; set; }

        public List<MeterReading> MeterReadings { get; set; }
    }
}