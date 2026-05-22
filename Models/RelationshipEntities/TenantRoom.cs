using RentManagementApp.Models.MasterEntities;
using RentManagementApp.Models.TransactionalEntities;

namespace RentManagementApp.Models.RelationshipEntities
{
    public class TenantRoom
    {
        public int Id { get; set; }

        public int TenantId { get; set; }

        public int RoomId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public Tenant Tenant { get; set; } = null!;

        public Room Room { get; set; } = null!;
    }
}