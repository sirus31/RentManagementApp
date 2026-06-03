using RentManagementApp.Models.RelationshipEntities;
using RentManagementApp.Models.TransactionalEntities;

namespace RentManagementApp.Models.MasterEntities
{
    public class Tenant
    {
        public int Id { get; set; }

        public string FullName { get; set; }
            = string.Empty;

        public string PhoneNumber { get; set; }
            = string.Empty;

        public string? WhatsAppNumber { get; set; }

        public string? IdentificationNumber { get; set; }

        public string? PermanentAddress { get; set; }

        public DateTime JoinDate { get; set; }

        public DateTime? LeaveDate { get; set; }

        public bool IsActive { get; set; }

        public decimal MonthlyRent { get; set; }

        public List<TenantRoom> TenantRooms { get; set; }
            = new();

        public List<TenantMeter> TenantMeters { get; set; } 
            = new();

        public List<Bill> Bills { get; set; }
            = new();

    }
}