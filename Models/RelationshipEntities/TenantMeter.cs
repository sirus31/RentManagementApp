using RentManagementApp.Models.MasterEntities;
using RentManagementApp.Models.TransactionalEntities;

namespace RentManagementApp.Models.RelationshipEntities
{
    public class TenantMeter
    {
        public int Id { get; set; }

        public int TenantId { get; set; }

        public int MeterId { get; set; }

        public bool IsActive { get; set; }

        public Tenant Tenant { get; set; }

        public Meter Meter { get; set; }
    }
}