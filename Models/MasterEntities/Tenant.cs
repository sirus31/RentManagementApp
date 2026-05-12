using RentManagementApp.Models.RelationshipEntities;
using RentManagementApp.Models.TransactionalEntities;

namespace RentManagementApp.Models.MasterEntities
{
    public class Tenant
    {
        public int Id { get; set; }

        public string FullName { get; set; }

        public string PhoneNumber { get; set; }

        public string WhatsAppNumber { get; set; }

        public string IdentificationNumber { get; set; }

        public string PermanentAddress { get; set; }

        public DateTime JoinDate { get; set; }

        public DateTime? LeaveDate { get; set; }

        public bool IsActive { get; set; }

        public decimal MonthlyRent { get; set; }

        public List<TenantRoom> TenantRooms { get; set; }

        public List<TenantMeter> TenantMeters { get; set; }

        public List<Bill> Bills { get; set; }
    }
}