using RentManagementApp.Models.MasterEntities;
using RentManagementApp.Models.TransactionalEntities;

namespace RentManagementApp.Models.RelationshipEntities
{
    public class TenantMeter
    {
        public int Id { get; set; }

        public int TenantId { get; set; }

        public int MeterId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public Tenant Tenant { get; set; } = null!;

        public Meter Meter { get; set; } = null!;
    }
}