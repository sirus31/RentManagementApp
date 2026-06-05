using RentManagementApp.Models.Enums;
using RentManagementApp.Models.RelationshipEntities;
using RentManagementApp.Models.TransactionalEntities;

namespace RentManagementApp.Models.MasterEntities
{
    public class Meter
    {
        public int Id { get; set; }

        public int HouseId { get; set; }

        public string MeterNumber { get; set; } = null!;

        public MeterType MeterType { get; set; }

        public decimal InitialReading { get; set; }

        public bool IsActive { get; set; }

        public House House { get; set; } = null!;

        public List<TenantMeter> TenantMeters
        { get; set; } = new();

        public List<MeterReading> MeterReadings
        { get; set; } = new();
    }
}