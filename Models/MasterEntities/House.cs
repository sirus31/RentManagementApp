using RentManagementApp.Models.RelationshipEntities;
using RentManagementApp.Models.TransactionalEntities;

namespace RentManagementApp.Models.MasterEntities
{
    public class House
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public decimal ElectricityRate { get; set; }

        public decimal GarbageFee { get; set; }

        public DateTime CreatedAt { get; set; }

        public User User { get; set; }

        public List<Floor> Floors { get; set; }
        public List<Meter> Meters { get; set; }
    }
}